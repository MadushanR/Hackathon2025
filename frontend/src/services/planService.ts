// src/services/planService.ts

import { PlanRequestDto, AcademicPlan } from '@/types/plan';

// IMPORTANT: Replace with your actual backend URL when running!
const API_BASE_URL = 'https://localhost:5001/api/Plan/generate';

/**
 * POST: /api/Plan/generate
 * Generates an academic plan based on student input and existing course data (fetched server-side).
 * @param data The goals and profile of the student.
 * @returns The structured AcademicPlan object.
 */
export const generatePlan = async (data: PlanRequestDto): Promise<AcademicPlan> => {
  // --- MOCK IMPLEMENTATION FOR LOCAL TESTING ---
  // If you are developing the front-end without the C# backend running, uncomment this block.
  /*
  console.log('Generating plan with data:', data);
  await new Promise(resolve => setTimeout(resolve, 1500)); // Simulate API delay

  const mockPlan: AcademicPlan = {
      courses: [
          { courseName: "Calculus III", recommendation: "Dedicate 8 hours of focused study time per week, focusing heavily on vector fields, using Khan Academy for visualization." },
          { courseName: "Modern Literature", recommendation: "Join a study group for critical essay review. Focus on structuring arguments clearly, addressing your weakness with large essays." }
      ],
      gradeBreakdown: [
          { courseName: "Calculus III", tips: "Break down complex problems into smaller, manageable steps. Use office hours to clarify integration techniques." },
          { courseName: "Modern Literature", tips: "Use the outline method for note-taking instead of transcription. Review notes immediately after class." }
      ],
      timeManagement: {
          recommendedSchedule: {
              Monday: "2 hours of focused study (Calculus III practice sets)",
              Tuesday: "3 hours of study (Data Structures lectures & coding)",
              Wednesday: "1 hour Literature reading; 1-hour essay outlining",
              Thursday: "2 hours review of all class notes; 1 hour gym/break",
              Friday: "2 hours deep work on weakest subject (e.g., Essay draft)",
              Saturday: "4 hours project/assignment work; 2 hours review of past week",
              Sunday: "Complete relaxation; pre-read material for Monday classes"
          }
      },
      studyMethods: {
          recommendedTechniques: [
              { name: "Pomodoro Technique", description: "Use 25-minute bursts of deep work followed by 5-minute breaks to fight procrastination." },
              { name: "Active Recall", description: "After reading a section, close the book and recite the main points aloud to improve retention." }
          ]
      },
      resources: {
          campus: "Visit the Math Tutoring Center bi-weekly and the Writing Center for every major essay draft.",
          online: "Use freeCodeCamp for coding exercises and YouTube channels like 3Blue1Brown for complex math visualization."
      },
      motivation: "Focus on the feeling of competence achieved when you successfully manage your time and excel in a previously challenging area. Visualize your future success!",
      innovativeSuggestions: "Implement a 'Weakness First' hour every morning where you tackle the hardest task first, improving momentum and confidence."
  };

  return mockPlan;
  */
  // --- END MOCK IMPLEMENTATION ---


  // --- REAL FETCH CODE (Uncomment and remove mock block for production) ---
  try {
    const response = await fetch(API_BASE_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });

    if (!response.ok) {
      const errorBody = await response.json();
      throw new Error(errorBody.error || `Plan generation failed: ${response.statusText}`);
    }

    // The C# controller returns the structured JSON plan object directly
    const plan = await response.json();
    return plan as AcademicPlan;
  } catch (error) {
    console.error("Error calling backend API:", error);
    throw new Error(`Failed to connect to the backend API at ${API_BASE_URL}`);
  }
};
