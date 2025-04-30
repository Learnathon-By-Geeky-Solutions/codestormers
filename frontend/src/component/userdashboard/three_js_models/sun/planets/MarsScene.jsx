import React, { useRef, useState } from "react";
import { Canvas, useFrame, useLoader } from "@react-three/fiber";
import { Sphere, Stars, OrbitControls, Html } from "@react-three/drei";
import * as THREE from "three";

// Import your textures
import marsTexture from "../../../../../assets/imgs/solar/mars.jpg";
import phobosTexture from "../../../../../assets/imgs/solar/moon.jpg"; // Use real Phobos texture if you have
import deimosTexture from "../../../../../assets/imgs/solar/moon.jpg"; // Same here for Deimos

const Mars = () => {
  const marsRef = useRef();
  const atmosphereRef = useRef();
  const texture = useLoader(THREE.TextureLoader, marsTexture);
  const [hovered, setHovered] = useState(false);

  useFrame(() => {
    if (marsRef.current) {
      marsRef.current.rotation.y += 0.0008;
    }
    if (atmosphereRef.current) {
      atmosphereRef.current.rotation.y += 0.0005;
    }
  });

  return (
    <group>
      {/* Mars */}
      <Sphere
        args={[3, 64, 64]}
        ref={marsRef}
        onPointerOver={() => setHovered(true)}
        onPointerOut={() => setHovered(false)}
      >
        <meshStandardMaterial
          map={texture}
          roughness={0.7}
          metalness={0.2}
        />
      </Sphere>

      {/* Thin Atmosphere (Mars atmosphere is very thin) */}
      <Sphere args={[3.05, 64, 64]} ref={atmosphereRef}>
        <meshStandardMaterial
          transparent
          opacity={0.08}
          emissive={"orange"}
          emissiveIntensity={0.3}
          side={THREE.BackSide}
          depthWrite={false}
        />
      </Sphere>

      {/* Hover Info */}
      {hovered && (
        <Html position={[0, 4, 0]} center>
          <div style={{
            background: "rgba(0, 0, 0, 0.7)",
            color: "white",
            padding: "6px 10px",
            borderRadius: "8px",
            fontSize: "14px",
            fontWeight: "500",
            backdropFilter: "blur(4px)"
          }}>
            🔴 Mars<br />
            The Red Planet
          </div>
        </Html>
      )}
    </group>
  );
};

const Phobos = () => {
  const phobosRef = useRef();
  const pivot = useRef();
  const texture = useLoader(THREE.TextureLoader, phobosTexture);
  const [hovered, setHovered] = useState(false);

  useFrame(({ clock }) => {
    if (pivot.current) {
      pivot.current.rotation.y = clock.getElapsedTime() * 0.8; // Fast orbit
    }
    if (phobosRef.current) {
      phobosRef.current.rotation.y += 0.002;
    }
  });

  return (
    <group ref={pivot}>
      <Sphere
        args={[0.4, 32, 32]}
        ref={phobosRef}
        position={[5, 0, 0]}
        onPointerOver={() => setHovered(true)}
        onPointerOut={() => setHovered(false)}
      >
        <meshStandardMaterial
          map={texture}
          roughness={1}
          metalness={0}
        />
      </Sphere>

      {/* Hover Info */}
      {hovered && (
        <Html position={[5, 1, 0]} center>
          <div style={{
            background: "rgba(0, 0, 0, 0.7)",
            color: "white",
            padding: "5px 8px",
            borderRadius: "8px",
            fontSize: "13px",
            fontWeight: "500",
            backdropFilter: "blur(4px)"
          }}>
            🛰️ Phobos<br />
            Mars' Largest Moon
          </div>
        </Html>
      )}
    </group>
  );
};

const Deimos = () => {
  const deimosRef = useRef();
  const pivot = useRef();
  const texture = useLoader(THREE.TextureLoader, deimosTexture);
  const [hovered, setHovered] = useState(false);

  useFrame(({ clock }) => {
    if (pivot.current) {
      pivot.current.rotation.y = clock.getElapsedTime() * 0.4; // Slower orbit
    }
    if (deimosRef.current) {
      deimosRef.current.rotation.y += 0.0015;
    }
  });

  return (
    <group ref={pivot}>
      <Sphere
        args={[0.3, 32, 32]}
        ref={deimosRef}
        position={[8, 0, 0]}
        onPointerOver={() => setHovered(true)}
        onPointerOut={() => setHovered(false)}
      >
        <meshStandardMaterial
          map={texture}
          roughness={1}
          metalness={0}
        />
      </Sphere>

      {/* Hover Info */}
      {hovered && (
        <Html position={[8, 1, 0]} center>
          <div style={{
            background: "rgba(0, 0, 0, 0.7)",
            color: "white",
            padding: "5px 8px",
            borderRadius: "8px",
            fontSize: "13px",
            fontWeight: "500",
            backdropFilter: "blur(4px)"
          }}>
            🛰️ Deimos<br />
            Mars' Smallest Moon
          </div>
        </Html>
      )}
    </group>
  );
};

const MarsScene = () => {
  return (
    <div style={{ width: "100vw", height: "50vh", background: "black", overflow: "hidden" }}>
      <Canvas camera={{ position: [0, 2, 8], fov: 70 }}>
        {/* Lights */}
        <ambientLight intensity={0.6} />
        <directionalLight position={[10, 10, 5]} intensity={1.5} />

        {/* Mars and Atmosphere */}
        <Mars />

        {/* Moons orbiting */}
        <Phobos />
        <Deimos />

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

export default MarsScene;
