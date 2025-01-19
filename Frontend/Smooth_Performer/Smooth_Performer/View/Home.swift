//
//  Home.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import SwiftUI

struct Home: View {
    var body: some View {
        ZStack{
            ScrollView{ // TODO: need a list view with a navigation view to make it more presentable
                VStack{
                    HStack{
                        Text("Hi")
                        // MARK: user.name
                        Text("User")
                        Spacer()
                    }
                    .font(.largeTitle)
                    .padding(.leading,30)
                    .padding(.top,30)
                    
                    HStack{
                        Text("Current GPA:")
                        // MARK: user.gpa
                        Text("GPA")
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
                    
                    // TODO: List view for courses
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
            }
                    
            VStack(){
                Spacer()
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
                        .padding(.bottom,30)
                }
                
            }
            
        }
    }
}

#Preview {
    Home()
}
