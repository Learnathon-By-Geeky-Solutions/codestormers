import React, { useRef } from "react";
import { Canvas, useFrame } from "@react-three/fiber";
import { OrbitControls, Stars, Ring } from "@react-three/drei";
import * as THREE from "three";
import Sun from "./solar_Model/Sun";
import Planet from "./solar_Model/Planet";

// Import textures for each planet
import mercuryTexture from "../assets/imgs/solar/mercury.jpeg";
import venusTexture from "../assets/imgs/solar/venus.jpeg";
import earthTexture from "../assets/imgs/solar/earth.jpg";
import marsTexture from "../assets/imgs/solar/mars.jpg";
import jupiterTexture from "../assets/imgs/solar/jupiter.jpeg";
import saturnTexture from "../assets/imgs/solar/saturn.jpg";
import uranusTexture from "../assets/imgs/solar/uranus.png";
import neptuneTexture from "../assets/imgs/solar/neptune.jpeg";
import CometScene from "./CommetScene";
import AsteroidBelt from "./solar_Model/AsteroidBeltScene";

const planets = [
  { name: "Mercury", textureSrc: mercuryTexture, size: 0.3, distance: 3, orbitalSpeed: 0.97, axialSpeed: 0.004, angle: Math.random() * Math.PI * 2, rotationDirection: 1 },
  { name: "Venus", textureSrc: venusTexture, size: 0.6, distance: 5, orbitalSpeed: 0.78, axialSpeed: -0.001, angle: Math.random() * Math.PI * 2, rotationDirection: -1 },
  { name: "Earth", textureSrc: earthTexture, size: 0.7, distance: 7, orbitalSpeed: 0.6, axialSpeed: 0.01, angle: Math.random() * Math.PI * 2, rotationDirection: 1 },
  { name: "Mars", textureSrc: marsTexture, size: 0.5, distance: 10, orbitalSpeed: 0.44, axialSpeed: 0.009, angle: Math.random() * Math.PI * 2, rotationDirection: 1 },
  { name: "Jupiter", textureSrc: jupiterTexture, size: 1.5, distance: 14, orbitalSpeed: 0.33, axialSpeed: 0.02, angle: Math.random() * Math.PI * 2, rotationDirection: 1 },
  { name: "Saturn", textureSrc: saturnTexture, size: 1.2, distance: 18, orbitalSpeed: 0.23, axialSpeed: 0.018, angle: Math.random() * Math.PI * 2, rotationDirection: 1, hasRings: true },
  { name: "Uranus", textureSrc: uranusTexture, size: 1, distance: 22, orbitalSpeed: 0.19, axialSpeed: 0.015, angle: Math.random() * Math.PI * 2, rotationDirection: 1, tilt: Math.PI * 0.82 },
  { name: "Neptune", textureSrc: neptuneTexture, size: 0.9, distance: 26, orbitalSpeed: 0.12, axialSpeed: 0.016, angle: Math.random() * Math.PI * 2, rotationDirection: 1 },
];

const OrbitPath = ({ distance }) => (
  <Ring
    args={[distance, distance + 0.01, 64]}
    rotation={[-Math.PI / 2, 0, 0]}
    position={[0, 0, 0]}
  >
    <meshBasicMaterial color="slate" side={THREE.DoubleSide} />
  </Ring>
);

const TwinklingStars = () => {
  const groupRef = useRef();

  useFrame(() => {
    groupRef.current.children.forEach((star, index) => {
      star.material.opacity = 0.5 + Math.sin(Date.now() * 0.001 + index) * 0.5;
    });
  });

  return (
    <group ref={groupRef}>
      {Array.from({ length: 1000 }).map((_, i) => (
        <mesh
          key={i}
          position={[
            (Math.random() - 0.5) * 200,
            (Math.random() - 0.5) * 200,
            (Math.random() - 0.5) * 200,
          ]}
        >
          <sphereGeometry args={[0.1, 8, 8]} />
          <meshBasicMaterial
            color={new THREE.Color(Math.random(), Math.random(), Math.random())}
            transparent
            opacity={Math.random()}
          />
        </mesh>
      ))}
    </group>
  );
};

const SolarSystem = () => {
  return (
    <div style={{ width: "100vw", height: "100vh", margin: 0, padding: 0 }}>
      <Canvas
        style={{ background: "black" }}
        camera={{ position: [0, 5, 15], fov: 50 }}
      >
        {/* Lighting */}
        <ambientLight intensity={0.2} />
        <pointLight position={[0, 0, 0]} intensity={5} color={0xffffff} />
        <directionalLight position={[10, 10, 10]} intensity={1} />
        <hemisphereLight
          intensity={0.3}
          color={0xffffff}
          groundColor={0x000000}
        />
        <Sun />

        {/* Planets and Orbit Paths */}
        {planets.map((p, index) => (
          <group key={index}>
            <Planet {...p} />
            <OrbitPath distance={p.distance} />
          </group>
        ))}
        {/* asteroid */}
        {/* <AsteroidBelt /> */}
        {/* Twinkling Stars */}
        <TwinklingStars />

        {/* Space Background */}
        <Stars
          count={3000}
          radius={300}
          depth={100}
          factor={6}
          saturation={0}
        />

        {/* Camera Controls */}
        <OrbitControls enableZoom={true} maxDistance={50} minDistance={5} />
      </Canvas>
    </div>
  );
};

export default SolarSystem;
