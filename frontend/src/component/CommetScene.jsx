import React, { useRef } from "react";
import { Canvas, useFrame, extend } from "@react-three/fiber";
import { OrbitControls, Stars } from "@react-three/drei";
import * as THREE from "three";
import { EffectComposer, Bloom } from "@react-three/postprocessing";

// Custom Shader Material for Comet Head
const CometHeadMaterial = () => {
  const vertexShader = `
    varying vec3 vPosition;
    void main() {
      vPosition = position;
      gl_Position = projectionMatrix * modelViewMatrix * vec4(position, 1.0);
    }
  `;

  const fragmentShader = `
    varying vec3 vPosition;
    uniform float uTime;

    float random(vec2 st) {
      return fract(sin(dot(st.xy, vec2(12.9898, 78.233))) * 43758.5453123);
    }

    float noise(vec2 st) {
      vec2 i = floor(st);
      vec2 f = fract(st);
      float a = random(i);
      float b = random(i + vec2(1.0, 0.0));
      float c = random(i + vec2(0.0, 1.0));
      float d = random(i + vec2(1.0, 1.0));
      vec2 u = f * f * (3.0 - 2.0 * f);
      return mix(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
    }

    void main() {
      float distanceFromCenter = length(vPosition); // Distance from the center
      float gradient = smoothstep(0.2, 1.0, distanceFromCenter); // Gradient from center to edge
      vec2 uv = vPosition.xy * 2.0;
      float turbulence = noise(uv * 5.0 + uTime * 0.5) * 0.2; // Add turbulence
      vec3 color = mix(vec3(1.0, 1.0, 1.0), vec3(0.5, 0.7, 1.0), gradient + turbulence); // White to blue gradient with turbulence
      gl_FragColor = vec4(color, 1.0);
    }
  `;

  const material = new THREE.ShaderMaterial({
    vertexShader,
    fragmentShader,
    uniforms: {
      uTime: { value: 0 },
    },
    transparent: true,
  });

  return material;
};

const Comet = () => {
  const cometRef = useRef();
  const tailRef = useRef();
  const rayRef = useRef();
  const pointLightRef = useRef();
  const headMaterialRef = useRef();

  useFrame(({ clock }) => {
    const t = clock.getElapsedTime();

    // Comet motion (elliptical orbit)
    cometRef.current.position.x = Math.sin(t * 0.5) * 10; // Horizontal movement
    cometRef.current.position.y = Math.cos(t * 0.5) * 2; // Vertical movement
    cometRef.current.position.z = Math.sin(t * 0.3) * 5; // Depth movement

    // Tail follows the comet head
    tailRef.current.position.copy(cometRef.current.position);

    // Tail rotation and scaling for a flowing effect
    tailRef.current.rotation.z = t * 0.2; // Slight rotation
    tailRef.current.scale.y = 1 + Math.sin(t * 0.5) * 0.2; // Dynamic scaling

    // Ray follows the comet head
    rayRef.current.position.copy(cometRef.current.position);
    rayRef.current.rotation.z = t * 0.2; // Slight rotation
    rayRef.current.scale.y = 1 + Math.sin(t * 0.5) * 0.2; // Dynamic scaling

    // Point light follows the comet
    pointLightRef.current.position.copy(cometRef.current.position);

    // Update shader time uniform
    if (headMaterialRef.current) {
      headMaterialRef.current.uniforms.uTime.value = t;
    }
  });

  return (
    <group>
      {/* Comet Head (Elliptical Shape) */}
      <mesh ref={cometRef} position={[-10, 0, 0]} scale={[1, 0.5, 1]}> {/* Scale to make it elliptical */}
        <sphereGeometry args={[0.5, 32, 32]} />
        <primitive ref={headMaterialRef} object={CometHeadMaterial()} /> {/* Custom shader material */}
      </mesh>

      {/* Comet Tail (Particle System) */}
      <mesh ref={tailRef} position={[-10, 0, -1]} rotation={[Math.PI / 2, 0, 0]}>
        <coneGeometry args={[0.5, 8, 32]} /> {/* Slimmer base, longer tail */}
        <meshStandardMaterial
          color="#87CEFA" // Bluish-white color
          emissive="#87CEFA" // Glow effect
          emissiveIntensity={1.5}
          transparent
          opacity={0.5} // Lower opacity for blurrier effect
          fog={true} // Enable fog for a softer look
        />
      </mesh>

      {/* Comet Ray (Glowing Beam) */}
      <mesh ref={rayRef} position={[-10, 0, -1]} rotation={[Math.PI / 2, 0, 0]}>
        <coneGeometry args={[0.2, 10, 32]} /> {/* Slimmer and longer */}
        <meshStandardMaterial
          color="#87CEFA" // Bluish-white color
          emissive="#87CEFA" // Glow effect
          emissiveIntensity={2}
          transparent
          opacity={0.3} // Lower opacity for a softer look
          fog={true} // Enable fog for blending
        />
      </mesh>

      {/* Comet Glow (Point Light) */}
      <pointLight ref={pointLightRef} color="white" intensity={2} distance={10} />
    </group>
  );
};

const CometScene = () => {
  return (
    <Canvas style={{ background: "black" }} camera={{ position: [0, 0, 20], fov: 50 }}>
      {/* Lighting */}
      <ambientLight intensity={0.1} />
      <pointLight position={[0, 0, 5]} intensity={0.5} />

      {/* Comet */}
      <Comet />

      {/* Stars Background */}
      <Stars radius={100} depth={50} count={5000} factor={4} saturation={0} fade />

      {/* Camera Controls */}
      <OrbitControls enableZoom={true} maxDistance={50} minDistance={5} />

      {/* Post-Processing (Bloom Effect) */}
      <EffectComposer>
        <Bloom intensity={1.5} luminanceThreshold={0.5} luminanceSmoothing={0.9} />
      </EffectComposer>
    </Canvas>
  );
};

export default CometScene;