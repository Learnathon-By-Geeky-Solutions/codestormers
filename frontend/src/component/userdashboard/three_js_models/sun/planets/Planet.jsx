import React, { useRef, useState } from "react";
import { useFrame, useLoader } from "@react-three/fiber";
import { Sphere, Html } from "@react-three/drei";
import * as THREE from "three";

const Planet = ({
  name,
  textureSrc,
  size,
  distance,
  orbitalSpeed,
  axialSpeed,
  angle,
  rotationDirection,
  tilt = 0,
  description,
  atmosphere,
}) => {
  const planetRef = useRef(null);
  const [hovered, setHovered] = useState(false);
  const texture = useLoader(THREE.TextureLoader, textureSrc);

  useFrame(({ clock }) => {
    const t = clock.getElapsedTime();

    if (planetRef.current) {
      // Orbital motion
      planetRef.current.position.x = Math.sin(t * orbitalSpeed + angle) * distance;
      planetRef.current.position.z = Math.cos(t * orbitalSpeed + angle) * distance;

      // Axial rotation
      planetRef.current.rotation.y += axialSpeed * rotationDirection;

      // Apply tilt
      if (tilt) {
        planetRef.current.rotation.x = tilt;
      }
    }
  });

  return (
    <group>
      <Sphere
        args={[size, 32, 32]}
        ref={planetRef}
        onPointerOver={() => setHovered(true)}
        onPointerOut={() => setHovered(false)}
      >
        <meshStandardMaterial
          map={texture}
          metalness={0.5}
          roughness={0.5}
          emissive={new THREE.Color(0xffffff)}
          emissiveIntensity={hovered ? 0.5 : 0}
        />
      </Sphere>

      {hovered && (
        <Html position={[0, size + 0.8, 0]} center distanceFactor={8}>
          <div
            style={{
              background: "rgba(0, 0, 0, 0.7)",
              color: "#fff",
              padding: "12px 16px",
              borderRadius: "10px",
              boxShadow: "0 0 20px rgba(0,0,0,0.6)",
              minWidth: "220px",
              fontSize: "14px",
              transition: "all 0.3s ease",
            }}
          >
            <h3 style={{ marginBottom: "8px", fontSize: "18px" }}>{name}</h3>
            <p style={{ marginBottom: "6px" }}>{description}</p>
            <p style={{ fontSize: "12px", opacity: 0.8 }}>
              <strong>Atmosphere:</strong> {atmosphere}
            </p>
          </div>
        </Html>
      )}
    </group>
  );
};

export default Planet;
