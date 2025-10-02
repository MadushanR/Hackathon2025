"use client";

import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import { Student } from "@/types";
import Navbar from "@/app/components/Navbar";

export default function Profile() {
  const router = useRouter();
  const [student, setStudent] = useState<Student | null>(null);
  const [shapes, setShapes] = useState<
    { top: string; left: string; animationDuration: string; animationDelay: string }[]
  >([]);

  useEffect(() => {
    const id = localStorage.getItem("studentId");
    if (!id) {
      router.push("/login");
      return;
    }

    const fetchStudent = async () => {
      const res = await fetch(`http://localhost:5244/api/student/${id}`);
      if (res.ok) setStudent(await res.json());
    };
    fetchStudent();

    // Floating shapes
    const newShapes = Array.from({ length: 12 }, () => ({
      top: `${Math.random() * 100}%`,
      left: `${Math.random() * 100}%`,
      animationDuration: `${4 + Math.random() * 4}s`,
      animationDelay: `${Math.random() * 3}s`,
    }));
    setShapes(newShapes);
  }, [router]);

  const handleUpdate = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!student) return;

    const res = await fetch(`http://localhost:5244/api/student/${student.studentId}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        firstName: student.firstName,
        lastName: student.lastName,
        email: student.email,
        password: student.password,
        gpa: student.gpa,
        desiredGPA: student.desiredGPA,
      }),
    });

    if (res.ok) alert("Profile updated!");
    else alert("Update failed");
  };

  const handleDelete = async () => {
    if (!student) return;
    const confirmDelete = confirm(
      "Are you sure you want to delete your profile? This action cannot be undone."
    );
    if (!confirmDelete) return;

    const res = await fetch(`http://localhost:5244/api/student/${student.studentId}`, {
      method: "DELETE",
    });

    if (res.ok) {
      alert("Profile deleted successfully.");
      localStorage.removeItem("studentId");
      router.push("/");
    } else {
      alert("Failed to delete profile.");
    }
  };

  if (!student) return <p className="text-white text-center mt-20">Loading...</p>;

  return (
    <div className="relative min-h-screen bg-gradient-to-r from-purple-500 via-pink-500 to-red-500 text-white overflow-hidden">
      {/* Navbar */}
      <Navbar />

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

      {/* Main content */}
      <div className="flex flex-col items-center justify-center mt-20 p-4">
        <div className="flex flex-col gap-6 w-full max-w-md z-10">
          <h1 className="text-4xl font-bold drop-shadow-lg animate-fadeIn text-center">
            Your Profile
          </h1>

          <form
            onSubmit={handleUpdate}
            className="flex flex-col gap-3 bg-white/20 p-6 rounded-lg shadow-md animate-fadeIn animate-delay-200"
          >
            <label>
              First Name
              <input
                type="text"
                placeholder="First Name"
                value={student.firstName}
                onChange={(e) => setStudent({ ...student, firstName: e.target.value })}
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
                onChange={(e) => setStudent({ ...student, lastName: e.target.value })}
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
                onChange={(e) => setStudent({ ...student, email: e.target.value })}
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
                onChange={(e) => setStudent({ ...student, password: e.target.value })}
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
                onChange={(e) => setStudent({ ...student, gpa: Number(e.target.value) })}
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
                value={student.desiredGPA || ""}
                onChange={(e) => setStudent({ ...student, desiredGPA: Number(e.target.value) })}
                className="w-full p-2 rounded text-black"
              />
            </label>

            <button className="bg-blue-500 hover:bg-blue-600 transition-all transform hover:scale-105 rounded py-2 text-white font-semibold shadow-md mt-2">
              Update Profile
            </button>
          </form>

          <button
            onClick={handleDelete}
            className="bg-red-500 hover:bg-red-600 transition-all transform hover:scale-105 rounded py-2 text-white font-semibold shadow-md mt-2"
          >
            Delete Profile
          </button>
        </div>
      </div>

      <style jsx>{`
        @keyframes float {
          0%, 100% {
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
        .animate-delay-200 {
          animation-delay: 0.2s;
        }
      `}</style>
    </div>
  );
}
