using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HJEngine.prim
{
    class QuadTree
    {
        public QuadTree upperLeft;
        public QuadTree upperRight;
        public QuadTree lowerLeft;
        public QuadTree lowerRight;
        public prim.Point pointA;
        public prim.Point pointB;
        private float minArea = 2;

        public QuadTree(prim.Point newPointA, prim.Point newPointB)
        {
            this.pointA = newPointA;
            this.pointB = newPointB;
        }

        public void AddNode2(prim.Point pointA, prim.Point pointB)
        {
            float curArea = (pointB.x - pointA.x)*(pointB.y-pointA.y);
            if(curArea > minArea)
            {
                //upperLeft = 
            }
            //prim.Point ulPoint = pointA;
            //prim.Point urPoint = new prim.Point(pointB.x / 2, pointA.y);
            //prim.Point llPoint = new prim.Point(pointA.x, pointB.y / 2);
            //prim.Point lrPoint = new prim.Point(pointB.x / 2, point.y / 2);


        }

        public void AddNode()
        {
            //this.upperLeft = new QuadTree();
            //this.upperRight = new QuadTree();
            //this.lowerLeft = new QuadTree();
            //this.lowerRight = new QuadTree();

        }

        public static void InitQuadTree(float maxWidth, float maxHeight)
        {
            //First tree
        }

    }
}
