export default function AtomLogo() {
  const orbits = [
    { rx: 34, ry: 14, duration: "3s", rotation: 30, gradient: "url(#redGradient)" },
    { rx: 36, ry: 18, duration: "3.5s", rotation: 90, gradient: "url(#blueGradient)" },
    { rx: 38, ry: 22, duration: "4s", rotation: 150, gradient: "url(#greenGradient)" },
  ];

  return (
    <div className="flex items-center text-white text-2xl font-bold">
      C
      <span className="relative w-16 h-16 flex items-center justify-center">
        <svg
          width="80"
          height="80"
          viewBox="0 0 100 100"
          xmlns="http://www.w3.org/2000/svg"
          className="animate-spin-slow"
        >
          {/* Gradient Definitions */}
          <defs>
            <linearGradient id="redGradient" x1="0%" y1="0%" x2="100%" y2="100%">
              <stop offset="0%" stopColor="#FF5733" />
              <stop offset="100%" stopColor="#FF0000" />
            </linearGradient>
            <linearGradient id="blueGradient" x1="0%" y1="0%" x2="100%" y2="100%">
              <stop offset="0%" stopColor="#3383FF" />
              <stop offset="100%" stopColor="#002EFF" />
            </linearGradient>
            <linearGradient id="greenGradient" x1="0%" y1="0%" x2="100%" y2="100%">
              <stop offset="0%" stopColor="#33FF99" />
              <stop offset="100%" stopColor="#00FF00" />
            </linearGradient>
          </defs>

          {/* Center Nucleus */}
          <circle cx="50" cy="50" r="6" fill="white" stroke="gray" strokeWidth="1.5" />

          {/* 3D Orbits with Precise Angles */}
          {orbits.map((orbit, index) => (
            <g key={index} transform={`rotate(${orbit.rotation} 50 50)`}>
              <ellipse
                cx="50"
                cy="50"
                rx={orbit.rx}
                ry={orbit.ry}
                stroke={orbit.gradient}
                strokeWidth="1.5"
                fill="none"
                opacity="0.8"
              />
              {/* Electron moving along the orbit */}
              <circle r="4" fill={orbit.gradient} stroke="black" strokeWidth="0.5">
                <animateMotion
                  dur={orbit.duration}
                  repeatCount="indefinite"
                  path={`M ${50 - orbit.rx},50 A ${orbit.rx},${orbit.ry} 0 1,1 ${50 + orbit.rx},50 A ${orbit.rx},${orbit.ry} 0 1,1 ${50 - orbit.rx},50`}
                />
              </circle>
            </g>
          ))}
        </svg>
      </span>
      SMOVERSE
    </div>
  );
}
