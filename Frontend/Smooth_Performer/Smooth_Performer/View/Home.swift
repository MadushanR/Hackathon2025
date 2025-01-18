//
//  Home.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import SwiftUI

struct Home: View {
    var body: some View {
        HStack{
            Text("Hi")
            // MARK: user.name
            Text("User")
            Spacer()
        }
        .font(.largeTitle)
        .padding(.leading,30)
        .padding(.top,50)
        
        HStack{
            Text("Current GPA:")
            // MARK: user.gpa
            Text("GPA")
            Spacer()
        }
        .font(.title3)
        .padding(.leading,30)
        
        // TODO: List view for courses
        ScrollView{
            GroupBox{
                Text("Course1")
            }
            .padding(10)
            GroupBox{
                Text("Course2")
            }
            .padding(10)
            GroupBox{
                Text("Course3")
            }
            .padding(10)
            GroupBox{
                Text("Course4")
            }
            .padding(10)
            GroupBox{
                Text("Course5")
            }
            .padding(10)
        }
        
        Button{
            // TODO: function for adding Course
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
                .padding(.top)
        }
        
        Spacer()
    }
}

#Preview {
    Home()
}
