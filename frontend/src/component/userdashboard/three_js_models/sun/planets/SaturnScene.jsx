import React, { useRef, useState } from "react";
import { Canvas, useFrame, useLoader } from "@react-three/fiber";
import { Sphere, Stars, OrbitControls, Html, Ring } from "@react-three/drei";
import * as THREE from "three";

// Import your textures
import saturnTexture from "../../../../../assets/imgs/solar/saturn.jpg";
import saturnRingTexture from "../../../../../assets/imgs/solar/satur_ring.jpg"; // You'll need a ring texture

const Saturn = () => {
  const saturnRef = useRef();
  const [hovered, setHovered] = useState(false);

  useFrame(() => {
    if (saturnRef.current) {
      saturnRef.current.rotation.y += 0.002;
    }
  });

  return (
    <group>
      {/* Saturn */}
      <Sphere
        args={[4, 64, 64]}
        ref={saturnRef}
        onPointerOver={() => setHovered(true)}
        onPointerOut={() => setHovered(false)}
      >
        <meshStandardMaterial
          map={useLoader(THREE.TextureLoader, saturnTexture)}
          roughness={0.8}
          metalness={0.3}
        />
      </Sphere>

      {/* Saturn's Rings */}
      <Ring
        args={[5, 8, 64]}
        rotation={[-Math.PI / 2, 0, 0]}
        receiveShadow
      >
        <meshStandardMaterial
          map={useLoader(THREE.TextureLoader, saturnRingTexture)}
          side={THREE.DoubleSide}
          transparent
          opacity={0.8}
        />
      </Ring>

      {/* Hover Info */}
      {hovered && (
        <Html position={[0, 6, 0]} center>
          <div style={{
            background: "rgba(0, 0, 0, 0.7)",
            color: "white",
            padding: "6px 10px",
            borderRadius: "8px",
            fontSize: "14px",
            fontWeight: "500",
            backdropFilter: "blur(4px)"
          }}>
            🪐 Saturn<br />
            The Ringed Planet
          </div>
        </Html>
      )}
    </group>
  );
};

const SaturnScene = () => {
  return (
    <div style={{ width: "100vw", height: "50vh", background: "black", overflow: "hidden" }}>
      <Canvas camera={{ position: [0, 2, 12], fov: 70 }}>
        <ambientLight intensity={0.5} />
        <pointLight position={[10, 10, 10]} intensity={1} />
        
        <Saturn />
        
        <Stars
          radius={300}
          depth={60}
          count={2000}
          factor={7}
          fade
        />
        
        <OrbitControls enableZoom={true} minDistance={8} maxDistance={20} />
      </Canvas>
    </div>
  );
};

export default SaturnScene;