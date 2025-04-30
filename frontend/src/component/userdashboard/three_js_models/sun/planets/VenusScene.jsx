import React, { useRef, useState } from "react";
import { Canvas, useFrame, useLoader } from "@react-three/fiber";
import { Sphere, Stars, OrbitControls, Html } from "@react-three/drei";
import * as THREE from "three";

import venusTexture from "../../../../../assets/imgs/solar/venus.jpeg";

const Venus = () => {
  const venusRef = useRef();
  const atmosphereRef = useRef();
  const [hovered, setHovered] = useState(false);

  useFrame(() => {
    if (venusRef.current) {
      venusRef.current.rotation.y += 0.001;
    }
    if (atmosphereRef.current) {
      atmosphereRef.current.rotation.y += 0.0003;
    }
  });

  return (
    <group>
      <Sphere
        args={[3, 64, 64]}
        ref={venusRef}
        onPointerOver={() => setHovered(true)}
        onPointerOut={() => setHovered(false)}
      >
        <meshStandardMaterial
          map={useLoader(THREE.TextureLoader, venusTexture)}
          roughness={0.9}
          metalness={0.1}
        />
      </Sphere>

      {/* Thick Venusian atmosphere */}
      <Sphere args={[3.2, 64, 64]} ref={atmosphereRef}>
        <meshStandardMaterial
          color="#e3a76b"
          transparent
          opacity={0.4}
          side={THREE.BackSide}
        />
      </Sphere>

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
            🟠 Venus<br />
            Earth's Twin
          </div>
        </Html>
      )}
    </group>
  );
};

const VenusScene = () => {
  return (
    <div style={{ width: "100vw", height: "50vh", background: "black", overflow: "hidden" }}>
      <Canvas camera={{ position: [0, 2, 8], fov: 70 }}>
        <ambientLight intensity={0.7} />
        <pointLight position={[10, 10, 10]} intensity={1.2} />
        
        <Venus />
        
        <Stars
          radius={300}
          depth={60}
          count={1500}
          factor={5}
          fade
        />
        
        <OrbitControls enableZoom={true} minDistance={6} maxDistance={12} />
      </Canvas>
    </div>
  );
};

export default VenusScene;