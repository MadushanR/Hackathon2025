"use client";

import Link from "next/link";
import { useRouter } from "next/navigation";

export default function Navbar() {
  const router = useRouter();

  const handleLogout = () => {
    localStorage.removeItem("studentId");
    router.push("/");
  };

  return (
    <nav className="flex justify-between items-center p-4 bg-gradient-to-r from-purple-500 via-pink-500 to-red-500 text-white shadow-md">
      <Link
        href="/dashboard"
        className="font-bold text-lg hover:text-yellow-200 transition-colors"
      >
        Dashboard
      </Link>

      <div className="flex gap-4">
        <Link
          href="/profile"
          className="hover:text-yellow-200 transition-colors font-semibold"
        >
          Profile
        </Link>

        <button
          onClick={handleLogout}
          className="bg-red-500 hover:bg-red-600 transition-all transform hover:scale-105 rounded px-3 py-1 font-semibold shadow-md"
        >
          Logout
        </button>
      </div>
    </nav>
  );
}
