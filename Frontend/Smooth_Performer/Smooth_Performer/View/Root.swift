//
//  Root.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-19.
//

import SwiftUI

struct Root: View {
    @State private var showSLView = true
    
    var body: some View {
        ZStack{
            NavigationStack{
                HomeTab(showSLView: $showSLView)
            }
        }
        .onAppear{
            let currentStudent = try? MainViewModel().getCurrentUser()
            self.showSLView = currentStudent == nil
        }
        .fullScreenCover(isPresented: $showSLView) {
            NavigationStack{
                LSTab(showSLView: $showSLView)
            }
        }
    }
}

#Preview {
    Root()
}
