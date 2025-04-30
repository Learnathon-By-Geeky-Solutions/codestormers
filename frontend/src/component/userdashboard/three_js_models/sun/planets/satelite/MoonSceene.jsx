import React from "react";
import { Canvas } from "@react-three/fiber";
import { Stars, OrbitControls } from "@react-three/drei";
import Satellite from "./Satellite";


const MoonScene = ({name,title,texture}) => {
  return (
    <div style={{ width: "100vw", height: "50vh", background: "black", overflow: "hidden" }}>
      <Canvas camera={{ position: [0, 2, 7], fov: 60 }}>
        <ambientLight intensity={0.5} />
        <pointLight position={[10, 10, 10]} intensity={1} />
        
        <Satellite
          name={name}
          title={title}
          texture={texture}
          size={1.5}
          position={[0, 0, 0]}
        />
        
        <Stars radius={200} depth={50} count={1000} factor={5} fade />
        <OrbitControls enableZoom={true} minDistance={3} maxDistance={10} />
      </Canvas>
    </div>
  );
};

export default MoonScene;
