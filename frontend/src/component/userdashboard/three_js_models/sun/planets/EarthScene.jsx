import React, { useRef } from "react";
import { Canvas, useFrame, useLoader } from "@react-three/fiber";
import { Sphere, Stars, OrbitControls } from "@react-three/drei";
import * as THREE from "three";

// Import your textures
import earthTexture from "../../../../../assets/imgs/solar/mars.jpg";
import moonTexture from "../../../../../assets/imgs/solar/moon.jpg"; // (fix: should be moon texture!)

const Earth = () => {
  const earthRef = useRef();
  const atmosphereRef = useRef();
  const texture = useLoader(THREE.TextureLoader, earthTexture);

  useFrame(() => {
    if (earthRef.current) {
      earthRef.current.rotation.y += 0.0008;
    }
    if (atmosphereRef.current) {
      atmosphereRef.current.rotation.y += 0.0005;
    }
  });

  return (
    <group>
      {/* Earth */}
      <Sphere args={[3, 64, 64]} ref={earthRef}>
        <meshStandardMaterial
          map={texture}
          roughness={0.6}
          metalness={0.2}
        />
      </Sphere>

      {/* Atmosphere */}
      <Sphere args={[3.1, 64, 64]} ref={atmosphereRef}>
        <meshStandardMaterial
          transparent
          opacity={0.2}
          emissive={"skyblue"}
          emissiveIntensity={0.8}
          side={THREE.BackSide}
        />
      </Sphere>
    </group>
  );
};

const Moon = () => {
  const moonRef = useRef();
  const pivot = useRef();
  const texture = useLoader(THREE.TextureLoader, moonTexture);

  useFrame(({ clock }) => {
    if (pivot.current) {
      pivot.current.rotation.y = clock.getElapsedTime() * 0.5;
    }
    if (moonRef.current) {
      moonRef.current.rotation.y += 0.001;
    }
  });

  return (
    <group ref={pivot}>
      <Sphere args={[0.6, 32, 32]} ref={moonRef} position={[6, 0, 0]}>
        <meshStandardMaterial 
          map={texture}
          roughness={1}
          metalness={0}
        />
      </Sphere>
    </group>
  );
};

const EarthScene = () => {
  return (
    <div style={{ width: "100vw", height: "50vh",background:"black", overflow: "hidden" }}>
      <Canvas camera={{ position: [0, 2, 8], fov: 70 }}>
        {/* Lights */}
        <ambientLight intensity={0.6} />
        <directionalLight position={[10, 10, 5]} intensity={1.5} />

        {/* Earth and Atmosphere */}
        <Earth />

        {/* Moon orbiting */}
        <Moon />

        {/* Background stars */}
        <Stars
          radius={300}
          depth={60}
          count={1500}
          factor={5}
          fade
        />

        {/* Camera Controls */}
        <OrbitControls enableZoom={true} minDistance={7} maxDistance={12} />
      </Canvas>
    </div>
  );
};

export default EarthScene;
