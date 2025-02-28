import React, { useRef, useMemo } from "react";
import { useFrame } from "@react-three/fiber";
import * as THREE from "three";

// Texture for asteroids
import asteroidTexture from "../../assets/imgs/solar/asteroid.jpg"; // Replace with your texture path

const AsteroidBelt = () => {
  const groupRef = useRef();

  // Generate random asteroid positions, sizes, and rotations
  const asteroids = useMemo(() => {
    const asteroidData = [];
    for (let i = 0; i < 500; i++) {
      const position = [
        (Math.random() - 0.5) * 20, // Spread along x-axis
        (Math.random() - 0.5) * 2,  // Spread along y-axis
        (Math.random() - 0.5) * 20, // Spread along z-axis
      ];
      const size = Math.random() * 0.2 + 0.1; // Random size between 0.1 and 0.3
      const rotation = [
        Math.random() * Math.PI * 2, // Random x rotation
        Math.random() * Math.PI * 2, // Random y rotation
        Math.random() * Math.PI * 2, // Random z rotation
      ];
      asteroidData.push({ position, size, rotation });
    }
    return asteroidData;
  }, []);

  // Create a custom geometry for irregular asteroid shapes
  const asteroidGeometry = useMemo(() => {
    const geometry = new THREE.BufferGeometry();
    const vertices = [];
    const indices = [];

    // Create a randomized shape
    for (let i = 0; i < 100; i++) {
      const x = (Math.random() - 0.5) * 1;
      const y = (Math.random() - 0.5) * 1;
      const z = (Math.random() - 0.5) * 1;
      vertices.push(x, y, z);
    }

    // Create faces (indices) for the geometry
    for (let i = 0; i < vertices.length / 3 - 2; i++) {
      indices.push(i, i + 1, i + 2);
    }

    geometry.setAttribute("position", new THREE.Float32BufferAttribute(vertices, 3));
    geometry.setIndex(indices);
    geometry.computeVertexNormals(); // Ensure proper lighting

    return geometry;
  }, []);

  useFrame(() => {
    groupRef.current.rotation.z += 0.0005; // Slowly rotate the asteroid belt
  });

  return (
    <group ref={groupRef}>
      {asteroids.map((asteroid, i) => (
        <mesh
          key={i}
          position={asteroid.position}
          rotation={asteroid.rotation}
          scale={[asteroid.size, asteroid.size, asteroid.size]}
        >
          {/* Use the custom geometry for irregular shapes */}
          <primitive object={asteroidGeometry} />
          <meshStandardMaterial
            map={new THREE.TextureLoader().load(asteroidTexture)} // Apply texture
            color="brown"
            roughness={0.8} // Make the surface less shiny
            metalness={0.1} // Slight metallic effect
          />
        </mesh>
      ))}
    </group>
  );
};

export default AsteroidBelt;