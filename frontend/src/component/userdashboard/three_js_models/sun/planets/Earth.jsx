import React from "react";
import Planet from "./Planet"; // ✅ Make sure you import your Planet component
import earthTexture from "../../../../../assets/imgs/solar/earth.jpg";

const Earth = () => {
  const planets = [
    {
      name: "Earth",
      textureSrc: earthTexture,
      size: 0.7,
      distance: 7,
      orbitalSpeed: 0.6 * 0.01, // Converted to realistic speed
      axialSpeed: 0.01,
      angle: Math.random() * Math.PI * 2,
      rotationDirection: 1,
      description: "Earth is the third planet from the Sun. Supports millions of species including humans. Surface is 71% water.",
      atmosphere: "78% Nitrogen, 21% Oxygen, 1% other gases."
    },
  ];

  return (
    <>
      {planets.map((planet, index) => (
        <group key={index}>
          <Planet {...planet} />
        </group>
      ))}
    </>
  );
};

export default Earth;
