"use client";

import { useState } from "react";
import { Course } from "@/types";

interface Props {
  studentId: number;
  onCourseAdded: (course: Course) => void;
}

export default function CourseForm({ studentId, onCourseAdded }: Props) {
  const [course, setCourse] = useState({
    courseName: "",
    semester: "",
    sectionId: 0,
    credits: 0,
    grade: 0,
  });

  const handleAddCourse = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const res = await fetch("http://localhost:5244/api/course", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ ...course, studentId }),
      });

      if (!res.ok) {
        const errorData = await res.json();
        alert("Error adding course: " + (errorData.error || res.statusText));
        return;
      }

      const addedCourse = await res.json();
      onCourseAdded(addedCourse);
      setCourse({ courseName: "", semester: "", sectionId: 0, credits: 0, grade: 0 });
    } catch (err) {
      console.error(err);
      alert("Failed to add course.");
    }
  };

  return (
    <form onSubmit={handleAddCourse} className="flex flex-col gap-4 bg-white/20 p-4 rounded-lg shadow-md animate-fadeIn">
      <label className="flex flex-col text-white font-semibold">
        Course Name
        <input
          type="text"
          placeholder="Enter course name"
          value={course.courseName}
          onChange={(e) => setCourse({ ...course, courseName: e.target.value })}
          required
          className="mt-1 p-2 rounded text-black"
        />
      </label>

      <label className="flex flex-col text-white font-semibold">
        Semester
        <input
          type="text"
          placeholder="Enter semester (e.g., Fall 2025)"
          value={course.semester}
          onChange={(e) => setCourse({ ...course, semester: e.target.value })}
          required
          className="mt-1 p-2 rounded text-black"
        />
      </label>

      <label className="flex flex-col text-white font-semibold">
        Section Id
        <input
          type="number"
          placeholder="Enter section ID"
          value={course.sectionId}
          onChange={(e) => setCourse({ ...course, sectionId: Number(e.target.value) })}
          required
          className="mt-1 p-2 rounded text-black"
        />
      </label>

      <label className="flex flex-col text-white font-semibold">
        Credits
        <input
          type="number"
          placeholder="Enter credits"
          value={course.credits}
          onChange={(e) => setCourse({ ...course, credits: Number(e.target.value) })}
          required
          className="mt-1 p-2 rounded text-black"
        />
      </label>

      <label className="flex flex-col text-white font-semibold">
        Grade
        <input
          type="number"
          placeholder="Enter grade (0-100)"
          value={course.grade}
          onChange={(e) => setCourse({ ...course, grade: Number(e.target.value) })}
          required
          className="mt-1 p-2 rounded text-black"
        />
      </label>

      <button
        type="submit"
        className="bg-green-500 hover:bg-green-600 transition-all transform hover:scale-105 py-2 rounded text-white font-semibold shadow-md"
      >
        Add Course
      </button>

      <style jsx>{`
        @keyframes fadeIn {
          from { opacity: 0; transform: translateY(-10px); }
          to { opacity: 1; transform: translateY(0); }
        }
        .animate-fadeIn {
          animation: fadeIn 0.8s ease forwards;
        }
      `}</style>
    </form>
  );
}
