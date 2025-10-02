"use client";

import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";

export default function Login() {
  const router = useRouter();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  // Shapes state
  const [shapes, setShapes] = useState<
    { top: string; left: string; animationDuration: string; animationDelay: string }[]
  >([]);

  // Generate shapes only on client
  useEffect(() => {
    const newShapes = Array.from({ length: 8 }, () => ({
      top: `${Math.random() * 100}%`,
      left: `${Math.random() * 100}%`,
      animationDuration: `${4 + Math.random() * 4}s`,
      animationDelay: `${Math.random() * 3}s`,
    }));
    setShapes(newShapes);
  }, []);

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const res = await fetch("http://localhost:5244/api/student/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });

      if (!res.ok) {
        const errorData = await res.json();
        alert("Login failed: " + (errorData.error || res.statusText));
        return;
      }

      const data = await res.json();
      localStorage.setItem("studentId", data.studentId);
      router.push("/dashboard");
    } catch (err) {
      console.error(err);
      alert("Login failed");
    }
  };

  return (
    <div className="relative flex flex-col items-center justify-center min-h-screen text-white bg-gradient-to-r from-blue-500 via-purple-500 to-pink-500 overflow-hidden">
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
        <h1 className="text-4xl font-bold drop-shadow-lg animate-fadeIn">Student Login</h1>
        <form
          onSubmit={handleLogin}
          className="flex flex-col gap-4 w-80 bg-white/20 backdrop-blur-md p-6 rounded-xl shadow-lg animate-fadeIn animate-delay-300"
        >
          <input
            placeholder="Email"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            className="px-4 py-2 rounded bg-white/90 text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-400"
          />
          <input
            placeholder="Password"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            className="px-4 py-2 rounded bg-white/90 text-gray-800 focus:outline-none focus:ring-2 focus:ring-blue-400"
          />
          <button
            type="submit"
            className="bg-blue-600 hover:bg-blue-700 transition-all transform hover:scale-105 py-2 rounded text-white font-semibold shadow-md"
          >
            Login
          </button>
        </form>

        <Link href="/register">
          <button className="mt-2 bg-green-500 hover:bg-green-600 transition-all transform hover:scale-105 py-2 px-6 rounded text-white font-semibold shadow-md">
            Register
          </button>
        </Link>
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
