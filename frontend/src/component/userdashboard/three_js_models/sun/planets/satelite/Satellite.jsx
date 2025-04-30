import React, { useRef, useState } from "react";
import { useFrame, useLoader } from "@react-three/fiber";
import { Sphere, Html } from "@react-three/drei";
import * as THREE from "three";

const Satellite = ({ name, title, texture, size = 1, position = [0, 0, 0] }) => {
  const ref = useRef();
  const [hovered, setHovered] = useState(false);

  const textureMap = useLoader(THREE.TextureLoader, texture);

  useFrame(() => {
    if (ref.current) {
      ref.current.rotation.y += 0.005;
    }
  });

  return (
    <group position={position}>
      <Sphere
        args={[size, 64, 64]}
        ref={ref}
        onPointerOver={() => setHovered(true)}
        onPointerOut={() => setHovered(false)}
      >
        <meshStandardMaterial map={textureMap} roughness={0.7} />
      </Sphere>

      {hovered && (
        <Html position={[0, size + 0.5, 0]} center>
          <div style={{
            background: "rgba(0, 0, 0, 0.7)",
            color: "white",
            padding: "6px 10px",
            borderRadius: "8px",
            fontSize: "14px",
            fontWeight: "500",
            backdropFilter: "blur(4px)"
          }}>
            🛰️
            {title}
          </div>
        </Html>
      )}
    </group>
  );
};

export default Satellite;
