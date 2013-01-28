// This file is generated by WOK (CPPExt).
// Please do not edit this file; modify original file instead.
// The copyright and license terms as defined for the original file apply to 
// this header file considered to be the "object code" form of the original source.

#ifndef _Bnd_BoundSortBox_HeaderFile
#define _Bnd_BoundSortBox_HeaderFile

#ifndef _Standard_HeaderFile
#include <Standard.hxx>
#endif
#ifndef _Standard_DefineAlloc_HeaderFile
#include <Standard_DefineAlloc.hxx>
#endif
#ifndef _Standard_Macro_HeaderFile
#include <Standard_Macro.hxx>
#endif

#ifndef _Bnd_Box_HeaderFile
#include <Bnd_Box.hxx>
#endif
#ifndef _Handle_Bnd_HArray1OfBox_HeaderFile
#include <Handle_Bnd_HArray1OfBox.hxx>
#endif
#ifndef _Standard_Real_HeaderFile
#include <Standard_Real.hxx>
#endif
#ifndef _Standard_Integer_HeaderFile
#include <Standard_Integer.hxx>
#endif
#ifndef _TColStd_DataMapOfIntegerInteger_HeaderFile
#include <TColStd_DataMapOfIntegerInteger.hxx>
#endif
#ifndef _TColStd_ListOfInteger_HeaderFile
#include <TColStd_ListOfInteger.hxx>
#endif
#ifndef _Standard_Address_HeaderFile
#include <Standard_Address.hxx>
#endif
class Bnd_HArray1OfBox;
class Standard_NullValue;
class Standard_MultiplyDefined;
class Bnd_Box;
class TColStd_ListOfInteger;
class gp_Pln;


//! A tool to compare a bounding box or a plane with a set of <br>
//! bounding boxes. It sorts the set of bounding boxes to give <br>
//! the list of boxes which intersect the element being compared. <br>
//! The boxes being sorted generally bound a set of shapes, <br>
//! while the box being compared bounds a shape to be <br>
//! compared. The resulting list of intersecting boxes therefore <br>
//! gives the list of items which potentially intersect the shape to be compared. <br>
class Bnd_BoundSortBox  {
public:

  DEFINE_STANDARD_ALLOC

  //! Constructs an empty comparison algorithm for bounding boxes. <br>
//! The bounding boxes are then defined using the Initialize function. <br>
  Standard_EXPORT   Bnd_BoundSortBox();
  //! Initializes this comparison algorithm with <br>
//! -   the set of bounding boxes SetOfBox. <br>
  Standard_EXPORT     void Initialize(const Bnd_Box& CompleteBox,const Handle(Bnd_HArray1OfBox)& SetOfBox) ;
  //! Initializes this comparison algorithm with <br>
//! -   the set of bounding boxes SetOfBox, where <br>
//!   CompleteBox is given as the global bounding box of SetOfBox. <br>
  Standard_EXPORT     void Initialize(const Handle(Bnd_HArray1OfBox)& SetOfBox) ;
  //! Initializes this comparison algorithm, giving it only <br>
//! -   the maximum number nbComponents <br>
//! of the bounding boxes to be managed. Use the Add <br>
//! function to define the array of bounding boxes to be sorted by this algorithm. <br>
  Standard_EXPORT     void Initialize(const Bnd_Box& CompleteBox,const Standard_Integer nbComponents) ;
  //! Adds the bounding box theBox at position boxIndex in <br>
//! the array of boxes to be sorted by this comparison algorithm. <br>
//! This function is used only in conjunction with the third <br>
//! syntax described in the synopsis of Initialize. <br>
  Standard_EXPORT     void Add(const Bnd_Box& theBox,const Standard_Integer boxIndex) ;
  //! Compares the bounding box theBox, <br>
//! with the set of bounding boxes to be sorted by this <br>
//! comparison algorithm, and returns the list of intersecting <br>
//! bounding boxes as a list of indexes on the array of <br>
//! bounding boxes used by this algorithm. <br>
  Standard_EXPORT    const TColStd_ListOfInteger& Compare(const Bnd_Box& theBox) ;
  //! Compares the plane P <br>
//! with the set of bounding boxes to be sorted by this <br>
//! comparison algorithm, and returns the list of intersecting <br>
//! bounding boxes as a list of indexes on the array of <br>
//! bounding boxes used by this algorithm. <br>
  Standard_EXPORT    const TColStd_ListOfInteger& Compare(const gp_Pln& P) ;
  
  Standard_EXPORT     void Dump() const;
  
  Standard_EXPORT     void Destroy() ;
~Bnd_BoundSortBox()
{
  Destroy();
}





protected:





private:

  //! Prepares  BoundSortBox and  sorts   the  boxes of <br>
//!          <SetOfBox> . <br>
  Standard_EXPORT     void SortBoxes() ;


Bnd_Box myBox;
Handle_Bnd_HArray1OfBox myBndComponents;
Standard_Real Xmin;
Standard_Real Ymin;
Standard_Real Zmin;
Standard_Real deltaX;
Standard_Real deltaY;
Standard_Real deltaZ;
Standard_Integer discrX;
Standard_Integer discrY;
Standard_Integer discrZ;
Standard_Integer theFound;
TColStd_DataMapOfIntegerInteger Crible;
TColStd_ListOfInteger lastResult;
Standard_Address TabBits;


};





// other Inline functions and methods (like "C++: function call" methods)


#endif
