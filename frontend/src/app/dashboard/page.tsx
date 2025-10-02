"use client";

import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import Navbar from "@/app/components/Navbar";
import CourseForm from "@/app/components/CourseForm";
import PlanGenerator from "@/app/components/PlanGenerator";
import CourseList from "@/app/components/CourseList";
import { Course, Student } from "@/types";

export default function Dashboard() {
  const router = useRouter();
  const [studentId, setStudentId] = useState<number | null>(null);
  const [student, setStudent] = useState<Student | null>(null);
  const [courses, setCourses] = useState<Course[]>([]);

  // Fetch student and courses
  useEffect(() => {
    const id = localStorage.getItem("studentId");
    if (!id) {
      router.push("/login");
      return;
    }
    setStudentId(Number(id));

    const fetchStudent = async () => {
      const res = await fetch(`http://localhost:5244/api/student/${id}`);
      if (res.ok) setStudent(await res.json());
    };

    const fetchCourses = async () => {
      const res = await fetch(`http://localhost:5244/api/course/${id}`);
      if (res.ok) setCourses(await res.json());
    };

    fetchStudent();
    fetchCourses();
  }, [router]);

  const handleCourseAdded = (course: Course) => setCourses((prev) => [...prev, course]);
  const handleCourseUpdated = (updatedCourse: Course) =>
    setCourses((prev) => prev.map((c) => (c.courseId === updatedCourse.courseId ? updatedCourse : c)));
  const handleCourseDeleted = (id: number) => setCourses((prev) => prev.filter((c) => c.courseId !== id));

  if (!studentId || !student) return <p className="text-white">Loading...</p>;

  return (
    <div className="relative min-h-screen bg-gradient-to-r from-purple-500 via-pink-500 to-red-500 text-white overflow-hidden">
      <Navbar />

      <div className="p-6 flex flex-col gap-6 z-10 max-w-4xl mx-auto">
        <h1 className="text-4xl font-bold drop-shadow-lg animate-fadeIn">Welcome, {student.firstName}</h1>

        {/* Courses Section */}
        <section className="bg-white/20 p-4 rounded-lg shadow-md animate-fadeIn animate-delay-200">
        <span className="text-white">Current GPA : {student.gpa} </span><br />
        <span className="text-white">Desired GPA : {student.desiredGPA} </span>
          <h2 className="text-2xl mb-3">Your Courses</h2>
          <CourseList
            courses={courses}
            onCourseUpdated={handleCourseUpdated}
            onCourseDeleted={handleCourseDeleted}
          />
        </section>

        {/* Add Course Form */}
        <section className="bg-white/20 p-4 rounded-lg shadow-md animate-fadeIn animate-delay-300">
          <h2 className="text-2xl mb-3">Add a Course</h2>
          <CourseForm studentId={studentId} onCourseAdded={handleCourseAdded} />
        </section>

        {/* Academic Plan Generator */}
        <section className="bg-white/20 p-4 rounded-lg shadow-md animate-fadeIn animate-delay-400">
          <h2 className="text-2xl mb-3">Academic Plan</h2>
          <PlanGenerator
            studentId={studentId}
            currentGPA={student.gpa}
            desiredGPA={student.desiredGPA || student.gpa}
            totalCreditsNeeded={120} // example total credits
            strengths="Math, Programming"
            weaknesses="Time management"
          />
        </section>
      </div>

      <style jsx>{`
        @keyframes fadeIn {
          from { opacity: 0; transform: translateY(-15px); }
          to { opacity: 1; transform: translateY(0); }
        }
        .animate-fadeIn { animation: fadeIn 0.8s ease forwards; }
        .animate-delay-200 { animation-delay: 0.2s; }
        .animate-delay-300 { animation-delay: 0.3s; }
        .animate-delay-400 { animation-delay: 0.4s; }
      `}</style>
    </div>
  );
}
