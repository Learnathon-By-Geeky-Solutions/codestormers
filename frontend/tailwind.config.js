/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      animation: {
        float: 'float 15s ease-in-out infinite',
        'float-fast': 'float-fast 10s ease-in-out infinite',
      },
      keyframes: {
        float: {
          '0%, 100%': { transform: 'translateY(0) translateX(0) rotate(0deg)' },
          '25%': { transform: 'translateY(-20px) translateX(10px) rotate(5deg)' },
          '50%': { transform: 'translateY(0) translateX(20px) rotate(0deg)' },
          '75%': { transform: 'translateY(20px) translateX(10px) rotate(-5deg)' },
        },
        'float-fast': {
          '0%, 100%': { transform: 'translateY(0) translateX(0)' },
          '50%': { transform: 'translateY(-30px) translateX(30px)' },
        },
      },
    },
  },
  plugins: [],
}