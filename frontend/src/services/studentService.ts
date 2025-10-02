// src/services/studentService.ts

import {
  Student,
  LoginDto,
  RegisterStudentDto,
  UpdateStudentDto,
  GpaBySemester,
} from '@/types/student';

// IMPORTANT: Replace with your actual backend URL!
const API_BASE_URL = 'https://localhost:5001/api/Student'; // Example .NET URL

// --- Authentication Endpoints ---

/**
 * POST: /api/Student/register
 * Registers a new student.
 */
export const registerStudent = async (data: RegisterStudentDto): Promise<Student> => {
  const response = await fetch(`${API_BASE_URL}/register`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    // Attempt to read the error message from the response body
    const errorBody = await response.json();
    throw new Error(errorBody.error || `Registration failed with status: ${response.status}`);
  }

  return response.json();
};

/**
 * POST: /api/Student/login
 * Logs in a student and returns the Student object on success.
 */
export const loginStudent = async (data: LoginDto): Promise<Student> => {
  const response = await fetch(`${API_BASE_URL}/login`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    const errorBody = await response.json();
    throw new Error(errorBody.error || `Login failed with status: ${response.status}`);
  }

  return response.json();
};

// --- CRUD Endpoints ---

/**
 * GET: /api/Student
 * Retrieves a list of all students.
 */
export const getAllStudents = async (): Promise<Student[]> => {
  const response = await fetch(API_BASE_URL);

  if (!response.ok) {
    throw new Error(`Failed to fetch students: ${response.statusText}`);
  }

  return response.json();
};

/**
 * GET: /api/Student/{id}
 * Retrieves a single student by ID.
 */
export const getStudentById = async (id: number): Promise<Student> => {
  const response = await fetch(`${API_BASE_URL}/${id}`);

  if (response.status === 404) {
    throw new Error(`Student with ID ${id} not found.`);
  }
  if (!response.ok) {
    throw new Error(`Failed to fetch student: ${response.statusText}`);
  }

  return response.json();
};

/**
 * POST: /api/Student
 * Creates a new student (admin-style creation).
 */
export const createStudent = async (data: Student): Promise<Student> => {
  const response = await fetch(API_BASE_URL, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    throw new Error(`Failed to create student: ${response.statusText}`);
  }

  // The .NET controller returns the created student object
  return response.json();
};

/**
 * PUT: /api/Student/{id}
 * Updates an existing student's information.
 */
export const updateStudent = async (id: number, data: UpdateStudentDto): Promise<void> => {
  const response = await fetch(`${API_BASE_URL}/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });

  // The .NET controller returns 204 NoContent on success
  if (!response.ok) {
    const errorBody = await response.json();
    throw new Error(errorBody.error || `Failed to update student: ${response.statusText}`);
  }
};

/**
 * DELETE: /api/Student/{id}
 * Deletes a student by ID.
 */
export const deleteStudent = async (id: number): Promise<void> => {
  const response = await fetch(`${API_BASE_URL}/${id}`, {
    method: 'DELETE',
  });

  // The .NET controller returns 204 NoContent on success
  if (!response.ok) {
    throw new Error(`Failed to delete student: ${response.statusText}`);
  }
};

// --- Specialized Endpoint ---

/**
 * GET: /api/Student/{studentId}/gpa
 * Calculates and retrieves GPA grouped by semester for a student.
 */
export const getGpaBySemester = async (studentId: number): Promise<GpaBySemester[]> => {
  const response = await fetch(`${API_BASE_URL}/${studentId}/gpa`);

  if (response.status === 404) {
    // The .NET controller returns a 404 if no courses are found
    throw new Error(`No courses found for student ID ${studentId}.`);
  }
  if (!response.ok) {
    throw new Error(`Failed to fetch GPA data: ${response.statusText}`);
  }

  return response.json();
};