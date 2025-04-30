import React, { useRef, useState } from "react";
import { Canvas, useFrame, useLoader } from "@react-three/fiber";
import { Sphere, Stars, OrbitControls, Html } from "@react-three/drei";
import * as THREE from "three";

import jupiterTexture from "../../../../../assets/imgs/solar/jupiter.jpeg";

const Jupiter = () => {
  const jupiterRef = useRef();
  const [hovered, setHovered] = useState(false);

  useFrame(() => {
    if (jupiterRef.current) {
      jupiterRef.current.rotation.y += 0.005; // Fast rotation (Jupiter has the shortest day)
    }
  });

  return (
    <group>
      <Sphere
        args={[5, 64, 64]}
        ref={jupiterRef}
        onPointerOver={() => setHovered(true)}
        onPointerOut={() => setHovered(false)}
      >
        <meshStandardMaterial
          map={useLoader(THREE.TextureLoader, jupiterTexture)}
          roughness={0.8}
          metalness={0.3}
        />
      </Sphere>

      {/* Great Red Spot marker */}
      <Sphere args={[0.2, 16, 16]} position={[4.8, 0.5, 0]}>
        <meshStandardMaterial color="#ff6b6b" emissive="#ff0000" emissiveIntensity={0.3} />
      </Sphere>

      {hovered && (
        <Html position={[0, 7, 0]} center>
          <div style={{
            background: "rgba(0, 0, 0, 0.7)",
            color: "white",
            padding: "6px 10px",
            borderRadius: "8px",
            fontSize: "14px",
            fontWeight: "500",
            backdropFilter: "blur(4px)"
          }}>
            🟠 Jupiter<br />
            The Gas Giant
          </div>
        </Html>
      )}
    </group>
  );
};

const JupiterScene = () => {
  return (
    <div style={{ width: "100vw", height: "50vh", background: "black", overflow: "hidden" }}>
      <Canvas camera={{ position: [0, 3, 12], fov: 70 }}>
        <ambientLight intensity={0.6} />
        <pointLight position={[10, 10, 10]} intensity={1.5} />
        
        <Jupiter />
        
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

export default JupiterScene;