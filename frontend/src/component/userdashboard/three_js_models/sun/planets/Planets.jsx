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
      {/* Planet Sphere with Texture */}
      <Sphere
        args={[size, 32, 32]}
        ref={planetRef}
       
      >
        <meshStandardMaterial
          map={texture}
          metalness={0.5}
          roughness={0.5}
          emissive={ new THREE.Color(0xffffff) }
          emissiveIntensity={hovered ? 0.5 : 0}
        />
      </Sphere>

      {/* Label on Hover */}
      {hovered && (
        <Html position={[0, size + 0.5, 0]} center>
          <div
            style={{
              color: "white",
              background: "rgba(0,0,0,0.7)",
              padding: "5px",
              borderRadius: "5px",
            }}
          >
            {name}
          </div>
        </Html>
      )}
    </group>
  );
};

export default Planet;