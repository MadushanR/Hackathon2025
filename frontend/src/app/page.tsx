"use client";

import { useState, useEffect } from "react";
import Link from "next/link";

export default function Home() {
  const [shapes, setShapes] = useState<
    { top: string; left: string; animationDuration: string; animationDelay: string }[]
  >([]);

  useEffect(() => {
    const newShapes = Array.from({ length: 12 }, () => ({
      top: `${Math.random() * 100}%`,
      left: `${Math.random() * 100}%`,
      animationDuration: `${4 + Math.random() * 4}s`,
      animationDelay: `${Math.random() * 3}s`,
    }));
    setShapes(newShapes);
  }, []);

  return (
    <div className="relative flex flex-col items-center justify-center min-h-screen text-white bg-gradient-to-r from-purple-500 via-pink-500 to-red-500 overflow-hidden">
      {/* Floating shapes */}
      {shapes.map((shape, i) => (
        <div
          key={i}
          className="absolute w-6 h-6 bg-white opacity-20 rounded-full animate-float"
          style={{
            top: shape.top,
            left: shape.left,
            animationDuration: shape.animationDuration,
            animationDelay: shape.animationDelay,
          }}
        />
      ))}

      <div className="flex flex-col items-center gap-6 px-4 z-10">
        <h1 className="text-5xl font-bold drop-shadow-lg animate-fadeIn">Welcome to GPA Predictor Portal</h1>
        <div className="flex gap-4 animate-fadeIn animate-delay-300">
          <Link href="/login">
            <button className="px-6 py-2 bg-blue-500 hover:bg-blue-600 transition-all transform hover:scale-105 rounded text-white font-semibold shadow-md">
              Login
            </button>
          </Link>
          <Link href="/register">
            <button className="px-6 py-2 bg-green-500 hover:bg-green-600 transition-all transform hover:scale-105 rounded text-white font-semibold shadow-md">
              Register
            </button>
          </Link>
        </div>
      </div>

      <style jsx>{`
        @keyframes float {
          0%, 100% { transform: translateY(0); }
          50% { transform: translateY(-15px); }
        }
        .animate-float {
          animation-name: float;
          animation-timing-function: ease-in-out;
          animation-iteration-count: infinite;
        }

        @keyframes fadeIn {
          from { opacity: 0; transform: translateY(-15px); }
          to { opacity: 1; transform: translateY(0); }
        }
        .animate-fadeIn {
          animation: fadeIn 1s ease forwards;
        }
        .animate-delay-300 { animation-delay: 0.3s; }
      `}</style>
    </div>
  );
}
