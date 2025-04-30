import React, { useRef, useState } from "react";
import { Canvas, useFrame, useLoader } from "@react-three/fiber";
import { Sphere, Stars, OrbitControls, Html, Ring } from "@react-three/drei";
import * as THREE from "three";

import uranusTexture from "../../../../../assets/imgs/solar/uranus.png";
import uranusRingTexture from "../../../../../assets/imgs/solar/urenus_ring.jpg";

const Uranus = () => {
  const uranusRef = useRef();
  const [hovered, setHovered] = useState(false);

  useFrame(() => {
    if (uranusRef.current) {
      uranusRef.current.rotation.y += 0.003;
    }
  });

  return (
    <group>
      <Sphere
        args={[3.5, 64, 64]}
        ref={uranusRef}
        onPointerOver={() => setHovered(true)}
        onPointerOut={() => setHovered(false)}
      >
        <meshStandardMaterial
          map={useLoader(THREE.TextureLoader, uranusTexture)}
          roughness={0.7}
          metalness={0.1}
        />
      </Sphere>

      {/* Uranus' Rings (much fainter than Saturn's) */}
      <Ring
        args={[4, 6, 64]}
        rotation={[-Math.PI / 2, 0, 0]}
      >
        <meshStandardMaterial
          map={useLoader(THREE.TextureLoader, uranusRingTexture)}
          side={THREE.DoubleSide}
          transparent
          opacity={0.3}
        />
      </Ring>

      {hovered && (
        <Html position={[0, 5, 0]} center>
          <div style={{
            background: "rgba(0, 0, 0, 0.7)",
            color: "white",
            padding: "6px 10px",
            borderRadius: "8px",
            fontSize: "14px",
            fontWeight: "500",
            backdropFilter: "blur(4px)"
          }}>
            🔵 Uranus<br />
            The Ice Giant
          </div>
        </Html>
      )}
    </group>
  );
};

const UranusScene = () => {
  return (
    <div style={{ width: "100vw", height: "50vh", background: "black", overflow: "hidden" }}>
      <Canvas camera={{ position: [0, 2, 10], fov: 70 }}>
        <ambientLight intensity={0.6} />
        <pointLight position={[10, 10, 10]} intensity={0.8} />
        
        <Uranus />
        
        <Stars
          radius={300}
          depth={60}
          count={2000}
          factor={7}
          fade
        />
        
        <OrbitControls enableZoom={true} minDistance={7} maxDistance={15} />
      </Canvas>
    </div>
  );
};

export default UranusScene;