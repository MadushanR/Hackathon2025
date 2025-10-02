"use client";

import { useState } from "react";
import { Course } from "@/types";

interface Props {
  courses: Course[];
  onCourseUpdated: (updated: Course) => void;
  onCourseDeleted: (id: number) => void;
}

export default function CourseList({ courses, onCourseUpdated, onCourseDeleted }: Props) {
  const [editCourseId, setEditCourseId] = useState<number | null>(null);
  const [editData, setEditData] = useState<Partial<Course>>({});

  const handleEditClick = (course: Course) => {
    setEditCourseId(course.courseId);
    setEditData(course);
  };

  const handleSave = async () => {
    if (!editCourseId) return;

    try {
      const res = await fetch(`http://localhost:5244/api/course/${editCourseId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(editData),
      });

      if (!res.ok) {
        alert("Failed to update course");
        return;
      }

      onCourseUpdated(editData as Course);
      setEditCourseId(null);
    } catch (err) {
      console.error(err);
      alert("Error updating course");
    }
  };

  const handleDelete = async (id: number) => {
    const confirmDelete = confirm("Are you sure you want to delete this course?");
    if (!confirmDelete) return;

    try {
      const res = await fetch(`http://localhost:5244/api/course/${id}`, {
        method: "DELETE",
      });

      if (!res.ok) {
        alert("Failed to delete course");
        return;
      }

      onCourseDeleted(id);
    } catch (err) {
      console.error(err);
      alert("Error deleting course");
    }
  };

  if (!courses || courses.length === 0) {
    return <p className="text-white/70 italic">No courses added yet.</p>;
  }

  return (
    <div className="overflow-x-auto rounded-lg shadow-md bg-white/20 p-4 animate-fadeIn">
      <table className="table-auto w-full border-collapse text-white">
        <thead>
          <tr className="bg-white/30 text-black">
            <th className="border px-4 py-2">Course Name</th>
            <th className="border px-4 py-2">Semester</th>
            <th className="border px-4 py-2">Section</th>
            <th className="border px-4 py-2">Credits</th>
            <th className="border px-4 py-2">Grade</th>
            <th className="border px-4 py-2">Actions</th>
          </tr>
        </thead>
        <tbody>
          {courses.map((c) => (
            <tr key={c.courseId} className="hover:bg-white/10 transition-colors">
              {editCourseId === c.courseId ? (
                <>
                  <td className="border px-4 py-2">
                    <input
                      className="w-full p-1 rounded text-black"
                      value={editData.courseName || ""}
                      onChange={(e) => setEditData({ ...editData, courseName: e.target.value })}
                    />
                  </td>
                  <td className="border px-4 py-2">
                    <input
                      className="w-full p-1 rounded text-black"
                      value={editData.semester || ""}
                      onChange={(e) => setEditData({ ...editData, semester: e.target.value })}
                    />
                  </td>
                  <td className="border px-4 py-2">
                    <input
                      className="w-full p-1 rounded text-black"
                      type="number"
                      value={editData.sectionId || 0}
                      onChange={(e) =>
                        setEditData({ ...editData, sectionId: Number(e.target.value) })
                      }
                    />
                  </td>
                  <td className="border px-4 py-2">
                    <input
                      className="w-full p-1 rounded text-black"
                      type="number"
                      value={editData.credits || 0}
                      onChange={(e) =>
                        setEditData({ ...editData, credits: Number(e.target.value) })
                      }
                    />
                  </td>
                  <td className="border px-4 py-2">
                    <input
                      className="w-full p-1 rounded text-black"
                      type="number"
                      value={editData.grade || 0}
                      onChange={(e) =>
                        setEditData({ ...editData, grade: Number(e.target.value) })
                      }
                    />
                  </td>
                  <td className="border px-4 py-2 flex gap-2">
                    <button
                      className="bg-green-500 px-2 py-1 rounded hover:bg-green-600"
                      onClick={handleSave}
                    >
                      Save
                    </button>
                    <button
                      className="bg-gray-500 px-2 py-1 rounded hover:bg-gray-600"
                      onClick={() => setEditCourseId(null)}
                    >
                      Cancel
                    </button>
                  </td>
                </>
              ) : (
                <>
                  <td className="border px-4 py-2">{c.courseName}</td>
                  <td className="border px-4 py-2">{c.semester}</td>
                  <td className="border px-4 py-2">{c.sectionId}</td>
                  <td className="border px-4 py-2">{c.credits}</td>
                  <td className="border px-4 py-2">{c.grade}</td>
                  <td className="border px-4 py-2 flex gap-2">
                    <button
                      className="bg-blue-500 px-2 py-1 rounded hover:bg-blue-600"
                      onClick={() => handleEditClick(c)}
                    >
                      Edit
                    </button>
                    <button
                      className="bg-red-500 px-2 py-1 rounded hover:bg-red-600"
                      onClick={() => handleDelete(c.courseId)}
                    >
                      Delete
                    </button>
                  </td>
                </>
              )}
            </tr>
          ))}
        </tbody>
      </table>

      <style jsx>{`
        @keyframes fadeIn {
          from {
            opacity: 0;
            transform: translateY(-10px);
          }
          to {
            opacity: 1;
            transform: translateY(0);
          }
        }
        .animate-fadeIn {
          animation: fadeIn 0.8s ease forwards;
        }
      `}</style>
    </div>
  );
}
