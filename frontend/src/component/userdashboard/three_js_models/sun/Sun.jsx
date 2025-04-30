import React, { useRef } from "react";
import { useLoader, useFrame } from "@react-three/fiber"; // ✅ Import useFrame
import * as THREE from "three";
import { Sphere } from "@react-three/drei";
import sunTexture from "../../../../assets/imgs/solar/sun.jpg"; // Ensure the correct path

const Sun = () => {
  const texture = useLoader(THREE.TextureLoader, sunTexture);
  const sunRef = useRef();

  useFrame(() => {
    if (sunRef.current) {
      sunRef.current.rotation.y += 0.002; // Slow self-rotation
    }
  });

  return (
    <Sphere args={[1.5, 32, 32]} ref={sunRef}>
      <meshStandardMaterial 
        map={texture} // Apply texture
        emissiveMap={texture} // Add emission effect
        emissive="orange" 
        emissiveIntensity={2} 
      />
    </Sphere>
  );
};

export default Sun;
