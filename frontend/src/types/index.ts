export interface Student {
  studentId: number;
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  gpa: number;
  desiredGPA?: number;
}

export interface Course {
  courseId: number;
  courseName: string;
  semester: string;
  sectionId: number;
  credits: number;
  grade: number;
  studentId: number;
}
