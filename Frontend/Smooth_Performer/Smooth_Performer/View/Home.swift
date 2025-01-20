//
//  Home.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import SwiftUI

struct Home: View {
    let vm = HomeVM()
    @State var addCourse:Bool = false
    @State var student:Student
    var body: some View {
        ZStack{
            // TODO: need a list view with a navigation view to make it more presentable
            VStack{
                HStack{
                    Text("Hi")
                    // MARK: user.name
                    Text("\(student.firstName) \(student.lastName)")
                    Spacer()
                }
                .font(.largeTitle)
                .padding(.leading,30)
                .padding(.top,30)
                
                HStack{
                    Text("Current GPA:")
                    // MARK: user.gpa
                    Text("\(student.gpa)")
                    Spacer()
                }
                .font(.title3)
                .padding(.leading,30)
                HStack{
                    Text("Review Planner")
                    Spacer()
                    Button{
                        // TODO: make a function to grab planner
                    }label: {
                        Image(systemName: "wand.and.rays")
                            .fontWeight(.bold)
                            .padding()
                            .background(Color.orange)
                            .foregroundColor(.white)
                            .cornerRadius(10)
                            .padding(.horizontal,10)
                    }
                }
                .font(.title3)
                .padding(.horizontal,30)
                
                if student.courses.isEmpty{
                    Text("No Courses")
                    
                }else{
                    List(student.courses){course in
                        GroupBox{
                            Text("\(course.courseName)")
                        }
                    }
                }


            }
            
            VStack(){
                Spacer()
                Button{
                    // TODO: function for adding Course
//                    showSLView = vm.signOut() //MARK: sign-Out
                    addCourse.toggle()
                    
                }label: {
                    Text("+ Add Course")
                        .font(.title3)
                        .fontWeight(.bold)
                        .frame(maxWidth: .infinity)
                        .padding()
                        .background(Color.orange)
                        .foregroundColor(.white)
                        .cornerRadius(10)
                        .padding(.horizontal,30)
                        .padding(.bottom,30)
                }
                
            }
            
        }
        .onAppear{
            Task{
                student.courses = await vm.loadCourses(id: student.studentId)
            }
        }
        .sheet(isPresented: $addCourse) {
            AddCourse(disableView: $addCourse)
        }
    }
}

#Preview {
    Home(student: FetchService().student!)
}
