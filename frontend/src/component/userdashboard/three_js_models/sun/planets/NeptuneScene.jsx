import React, { useRef, useState } from "react";
import { Canvas, useFrame, useLoader } from "@react-three/fiber";
import { Sphere, Stars, OrbitControls, Html } from "@react-three/drei";
import * as THREE from "three";

import neptuneTexture from "../../../../../assets/imgs/solar/neptune.jpeg";

const Neptune = () => {
  const neptuneRef = useRef();
  const [hovered, setHovered] = useState(false);

  useFrame(() => {
    if (neptuneRef.current) {
      neptuneRef.current.rotation.y += 0.004;
    }
  });

  return (
    <group>
      <Sphere
        args={[3.2, 64, 64]}
        ref={neptuneRef}
        onPointerOver={() => setHovered(true)}
        onPointerOut={() => setHovered(false)}
      >
        <meshStandardMaterial
          map={useLoader(THREE.TextureLoader, neptuneTexture)}
          roughness={0.8}
          metalness={0.2}
        />
      </Sphere>

      {/* Atmospheric effect */}
      <Sphere args={[3.25, 64, 64]}>
        <meshStandardMaterial
          color="#5b5ddf"
          transparent
          opacity={0.2}
          side={THREE.BackSide}
        />
      </Sphere>

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
            🔵 Neptune<br />
            The Windy Ice Giant
          </div>
        </Html>
      )}
    </group>
  );
};

const NeptuneScene = () => {
  return (
    <div style={{ width: "100vw", height: "50vh", background: "black", overflow: "hidden" }}>
      <Canvas camera={{ position: [0, 2, 10], fov: 70 }}>
        <ambientLight intensity={0.5} />
        <pointLight position={[10, 10, 10]} intensity={0.8} />
        
        <Neptune />
        
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

export default NeptuneScene;