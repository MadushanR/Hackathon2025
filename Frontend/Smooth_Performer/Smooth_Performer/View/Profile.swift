//
//  Profile.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import SwiftUI

struct Profile: View {
    let vm = ProfileVM()
    let student:Student
    @Binding var showSLView:Bool
    var body: some View {
        NavigationStack{
            
            HStack{
                VStack(alignment: .leading){
                    Text("\(student.firstName) \(student.lastName)")
                        .font(.largeTitle)
                    Text("\(student.studentId)")
                        .font(.title2)
                }
                Spacer()
            }
            .padding(35)
            
            VStack{
                if student.courses.isEmpty{
                    Text("No Courses")
                }else{
                    List(student.courses){course in
                        Text("\(course.courseName)")
                    }
                }
            }
            
            Spacer()
            
            Button{
                
            }label: {
                Text("Save")
                Spacer()
                Text(">")
            }
            .disabled(false)
            .font(.title3)
            .fontWeight(.bold)
            .frame(maxWidth: .infinity)
            .padding()
            .background(Color.orange)
            .foregroundColor(.white)
            .cornerRadius(10)
            .padding(.horizontal,40)
            .padding(.bottom,30)
            
            Button{
                showSLView = vm.signOut()
            }label: {
                Text("Log-Out")
            }
            .disabled(false)
            .font(.title3)
            .fontWeight(.bold)
            .frame(maxWidth: 90)
            .padding()
            .background(Color.orange)
            .foregroundColor(.white)
            .cornerRadius(10)
            .padding(.horizontal,40)
            
            Spacer()
                
        }
    }
}

#Preview {
    Profile(student: FetchService().student!, showSLView: .constant(true))
//    Profile()
}
