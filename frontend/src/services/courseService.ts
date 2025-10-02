// src/services/courseService.ts

import {
  Course,
  CreateCourseDto,
  UpdateCourseDto,
} from '@/types/course'; // Assuming you put types in course.ts

// IMPORTANT: Replace with your actual backend URL!
const API_BASE_URL = 'https://localhost:5001/api/Course';

// --- Read Endpoint ---

/**
 * GET: /api/Course/{studentId}
 * Retrieves all courses for a specific student.
 */
export const getCoursesByStudent = async (studentId: number): Promise<Course[]> => {
  const response = await fetch(`${API_BASE_URL}/${studentId}`);

  if (response.status === 404) {
    // Return empty array if no courses are found (as per the C# logic's 404 meaning)
    return []; 
  }

  if (!response.ok) {
    // Attempt to read error message if status is 500
    const errorBody = await response.json();
    throw new Error(errorBody.error || `Failed to fetch courses: ${response.statusText}`);
  }

  return response.json();
};

// --- Create Endpoint ---

/**
 * POST: /api/Course
 * Adds a new course for a student.
 */
export const addCourse = async (data: CreateCourseDto): Promise<Course> => {
  const response = await fetch(API_BASE_URL, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    const errorBody = await response.json();
    throw new Error(errorBody || `Failed to add course: ${response.statusText}`);
  }

  // The .NET controller returns the created Course object
  return response.json();
};

// --- Update Endpoint ---

/**
 * PUT: /api/Course/{id}
 * Updates an existing course by its CourseId.
 */
export const editCourse = async (id: number, data: UpdateCourseDto): Promise<void> => {
  const response = await fetch(`${API_BASE_URL}/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });

  // The .NET controller returns 204 NoContent on success
  if (!response.ok) {
    throw new Error(`Failed to update course: ${response.statusText}. Check console for details.`);
  }
};

// --- Delete Endpoint ---

/**
 * DELETE: /api/Course/{id}
 * Deletes a course by its CourseId.
 */
export const deleteCourse = async (id: number): Promise<void> => {
  const response = await fetch(`${API_BASE_URL}/${id}`, {
    method: 'DELETE',
  });

  // The .NET controller returns 204 NoContent on success
  if (!response.ok) {
    throw new Error(`Failed to delete course: ${response.statusText}`);
  }
};