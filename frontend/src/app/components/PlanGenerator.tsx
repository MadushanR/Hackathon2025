"use client";

import { useState } from "react";

interface Props {
  studentId: number;
  currentGPA: number;
  desiredGPA: number;
  totalCreditsNeeded: number;
  strengths: string;
  weaknesses: string;
}

interface Plan {
  courses: { courseName: string; recommendation: string }[];
  gradeBreakdown: { courseName: string; tips: string }[];
  timeManagement: { recommendedSchedule: Record<string, string> };
  studyMethods: { recommendedTechniques: { name: string; description: string }[] };
  resources: { campus: string; online: string };
  motivation: string;
  innovativeSuggestions: string;
}

export default function PlanGenerator({
  studentId,
  currentGPA,
  desiredGPA,
  totalCreditsNeeded,
  strengths,
  weaknesses,
}: Props) {
  const [plan, setPlan] = useState<Plan | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleGeneratePlan = async () => {
    setLoading(true);
    setError(null);
    setPlan(null);

    try {
      const res = await fetch("http://localhost:5244/api/plan/generate", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          studentId,
          currentGPA,
          desiredGPA,
          totalCreditsNeeded,
          strengths,
          weaknesses,
        }),
      });

      if (!res.ok) {
        const data = await res.json();
        setError(data?.error || "Failed to generate plan");
        setLoading(false);
        return;
      }

      const data: Plan = await res.json();
      setPlan(data);
    } catch (err: any) {
      setError(err.message || "Failed to generate plan");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="mt-4 bg-white/20 p-4 rounded-lg shadow-md">
      <button
        onClick={handleGeneratePlan}
        disabled={loading}
        className="bg-purple-500 hover:bg-purple-600 px-4 py-2 rounded text-white font-semibold"
      >
        {loading ? "Generating..." : "Generate Plan"}
      </button>

      {error && (
        <p className="mt-2 text-red-300 font-medium break-words">{error}</p>
      )}

      {plan && (
        <div className="mt-4 space-y-4 text-black">
          <h3 className="text-xl font-bold">Courses Recommendations</h3>
          <ul className="list-disc pl-5">
            {plan.courses.map((c, i) => (
              <li key={i}>
                <strong>{c.courseName}:</strong> {c.recommendation}
              </li>
            ))}
          </ul>

          <h3 className="text-xl font-bold">Grade Breakdown</h3>
          <ul className="list-disc pl-5">
            {plan.gradeBreakdown.map((g, i) => (
              <li key={i}>
                <strong>{g.courseName}:</strong> {g.tips}
              </li>
            ))}
          </ul>

          <h3 className="text-xl font-bold">Time Management</h3>
          <table className="table-auto border-collapse w-full text-black">
            <thead>
              <tr>
                {Object.keys(plan.timeManagement.recommendedSchedule).map(day => (
                  <th key={day} className="border px-2 py-1 bg-gray-200">{day}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              <tr>
                {Object.values(plan.timeManagement.recommendedSchedule).map((val, i) => (
                  <td key={i} className="border px-2 py-1">{val}</td>
                ))}
              </tr>
            </tbody>
          </table>

          <h3 className="text-xl font-bold">Study Methods</h3>
          <ul className="list-disc pl-5">
            {plan.studyMethods.recommendedTechniques.map((tech, i) => (
              <li key={i}>
                <strong>{tech.name}:</strong> {tech.description}
              </li>
            ))}
          </ul>

          <h3 className="text-xl font-bold">Resources</h3>
          <p><strong>Campus:</strong> {plan.resources.campus}</p>
          <p><strong>Online:</strong> {plan.resources.online}</p>

          <h3 className="text-xl font-bold">Motivation</h3>
          <p>{plan.motivation}</p>

          <h3 className="text-xl font-bold">Innovative Suggestions</h3>
          <p>{plan.innovativeSuggestions}</p>
        </div>
      )}
    </div>
  );
}
