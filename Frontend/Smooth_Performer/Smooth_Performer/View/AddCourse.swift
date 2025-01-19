//
//  AddCourse.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-19.
//

import SwiftUI

struct AddCourse: View {
    @State var courseID:String = ""
    var body: some View {
        Spacer()
        VStack(alignment: .leading){
            Text("Hi")
                .font(.largeTitle)
            Text("To add Course. Search with the course Id provided from your University/College.")
                .font(.title3)
        }
        .padding()
        
        Spacer()
        TextField("Course Id",text: $courseID)
            .keyboardType(.emailAddress)
            .autocorrectionDisabled()
            .autocapitalization(.none)
            .padding()
            .background(Color.white)
            .cornerRadius(8)
            .overlay(
                RoundedRectangle(cornerRadius: 8)
                    .stroke(Color.black, lineWidth: 2)
                    .opacity(0.4)
            )
            .padding(.horizontal,30)
            .padding(.top,9)
        
        Button{
            // ADD Course
        }label: {
            Text("+  Add")
                .font(.title3)
                .fontWeight(.bold)
                .frame(maxWidth: 80)
                .padding()
                .background(Color.orange)
                .foregroundColor(.white)
                .cornerRadius(10)
                .padding(.horizontal,30)
                .padding(.top)
        }
        
        Spacer()
    }
}

#Preview {
    AddCourse()
}
