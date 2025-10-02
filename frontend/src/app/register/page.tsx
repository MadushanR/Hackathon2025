// app/register/page.tsx
"use client";

import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";

export default function Register() {
  const router = useRouter();
  const [student, setStudent] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    gpa: "",        // Store as string for input
    desiredGPA: "", // Store as string for input
  });

  const [shapes, setShapes] = useState<
    { top: string; left: string; animationDuration: string; animationDelay: string }[]
  >([]);

  // Generate floating shapes on mount
  useEffect(() => {
    const newShapes = Array.from({ length: 12 }, () => ({
      top: `${Math.random() * 100}%`,
      left: `${Math.random() * 100}%`,
      animationDuration: `${4 + Math.random() * 4}s`,
      animationDelay: `${Math.random() * 3}s`,
    }));
    setShapes(newShapes);
  }, []);

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const res = await fetch("http://localhost:5244/api/student/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          firstName: student.firstName,
          lastName: student.lastName,
          email: student.email,
          password: student.password,
          gpa: Number(student.gpa),           // Convert to number for backend
          desiredGPA: Number(student.desiredGPA),
        }),
      });

      if (!res.ok) {
        const errorData = await res.json();
        alert("Error: " + (errorData.error || res.statusText));
        return;
      }

      const data = await res.json();
      localStorage.setItem("studentId", data.studentId);
      router.push("/dashboard");
    } catch (err) {
      console.error(err);
      alert("Registration failed");
    }
  };

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

      <div className="flex flex-col items-center gap-6 px-4 z-10 w-full max-w-md">
        <h1 className="text-4xl font-bold drop-shadow-lg animate-fadeIn">
          Register
        </h1>

        <form
          onSubmit={handleRegister}
          className="flex flex-col gap-3 w-full animate-fadeIn animate-delay-300"
        >
          <label>
            First Name
            <input
              type="text"
              placeholder="First Name"
              value={student.firstName}
              onChange={(e) =>
                setStudent({ ...student, firstName: e.target.value })
              }
              required
              className="w-full p-2 rounded text-black"
            />
          </label>

          <label>
            Last Name
            <input
              type="text"
              placeholder="Last Name"
              value={student.lastName}
              onChange={(e) =>
                setStudent({ ...student, lastName: e.target.value })
              }
              required
              className="w-full p-2 rounded text-black"
            />
          </label>

          <label>
            Email
            <input
              type="email"
              placeholder="Email"
              value={student.email}
              onChange={(e) =>
                setStudent({ ...student, email: e.target.value })
              }
              required
              className="w-full p-2 rounded text-black"
            />
          </label>

          <label>
            Password
            <input
              type="password"
              placeholder="Password"
              value={student.password}
              onChange={(e) =>
                setStudent({ ...student, password: e.target.value })
              }
              required
              className="w-full p-2 rounded text-black"
            />
          </label>

          <label>
            Current GPA
            <input
              type="number"
              step="0.01"
              placeholder="Current GPA"
              value={student.gpa}
              onChange={(e) =>
                setStudent({ ...student, gpa: e.target.value })
              }
              required
              className="w-full p-2 rounded text-black"
            />
          </label>

          <label>
            Desired GPA
            <input
              type="number"
              step="0.01"
              placeholder="Desired GPA"
              value={student.desiredGPA}
              onChange={(e) =>
                setStudent({ ...student, desiredGPA: e.target.value })
              }
              required
              className="w-full p-2 rounded text-black"
            />
          </label>

          <button className="bg-green-500 hover:bg-green-600 transition-all transform hover:scale-105 rounded py-2 text-white font-semibold shadow-md">
            Register
          </button>
        </form>

        <div className="mt-2">
          Already have an account?{" "}
          <Link href="/login">
            <button className="text-blue-300 hover:text-blue-500 font-semibold underline">
              Login
            </button>
          </Link>
        </div>
      </div>

      {/* Styles for animations */}
      <style jsx>{`
        @keyframes float {
          0%,
          100% {
            transform: translateY(0);
          }
          50% {
            transform: translateY(-15px);
          }
        }
        .animate-float {
          animation-name: float;
          animation-timing-function: ease-in-out;
          animation-iteration-count: infinite;
        }

        @keyframes fadeIn {
          from {
            opacity: 0;
            transform: translateY(-15px);
          }
          to {
            opacity: 1;
            transform: translateY(0);
          }
        }
        .animate-fadeIn {
          animation: fadeIn 1s ease forwards;
        }
        .animate-delay-300 {
          animation-delay: 0.3s;
        }
      `}</style>
    </div>
  );
}
