#pragma once
#include <windows.h>
#include <gl/gl.h>
#include <gl/glu.h>
using namespace System;
using namespace System::IO;


namespace Xbim
{
	namespace ModelGeometry
	{
		public delegate void BeginTessellateEventHandler(GLenum type);
		public delegate void EndTessellateEventHandler();
		public delegate void AddVertexEventHandler(IntPtr index);
		public delegate void TessellateErrorEventHandler(GLenum errNum);
		public interface class ITessellatorReceiver
		{
			void BeginTessellate(GLenum type);
			void EndTessellate();
			void TessellateError(GLenum errNum);
			void AddVertex(IntPtr index);
			void AddPoint(double X, double Y, double Z);
			void AddNormal(double X, double Y, double Z);
			void AddTriangleIndices(int a, int b, int c);
			void Clear();
			int PositionCount();
			property Stream^ DataStream { Stream^ get(); void set(Stream^ strm);};
		};
	}
}