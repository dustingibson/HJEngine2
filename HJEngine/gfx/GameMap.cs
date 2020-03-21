using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using OpenTK;
using Box2DX.Common;
using Box2DX.Dynamics;
using Box2DX.Collision;
using Box2DX;
using OpenTK.Input;
using MapInterface;

namespace HJEngine.gfx
{
    class GameMap : ContactListener
    {
        public MapInterface.MapInterface mapInterface;
        public ControlEntity controlEntity;
        public World world;
        public prim.Point scroll;
        private AABB worldAABB;
        private Graphics graphics;
        private bool run;

        public GameMap(Graphics graphics)
        {
            this.graphics = graphics;
            this.scroll = new prim.Point(0, 0);
            mapInterface = new MapInterface.MapInterface();
            controlEntity = new ControlEntity();
            run = true;
        }

        public void AddControlEntity(Graphics graphics, string templateKey)
        {
            MapInterface.ObjectTemplate template = mapInterface.objectTemplates[templateKey];
            controlEntity = new ControlEntity(world, template, graphics);
        }

        public void LoadMap(Graphics graphics, string path = "")
        {
            worldAABB = new AABB();
            worldAABB.LowerBound.Set(-5f, -5f);
            worldAABB.UpperBound.Set(5f, 5f);
            if (path == "")
                mapInterface.Load();
            else
                mapInterface = new MapInterface.MapInterface(path);
            //Offload Interface
            float gravityValue = mapInterface.objectTemplates["Global"].properties["Gravity"].getFloat();
            Vec2 gravity = new Vec2();
            gravity.Set(0f, gravityValue);
            world = new World(worldAABB, gravity, false);
            world.SetContactListener(this);

            List<MapInterface.ObjectInstance> tempInstance = new List<MapInterface.ObjectInstance>(mapInterface.objectInstances);
            mapInterface.objectInstances.Clear();
            //Convert to entities
            foreach (MapInterface.ObjectInstance curInstance in tempInstance)
            {
                prim.Point pnt = new prim.Point(curInstance.x, curInstance.y);
                mapInterface.objectInstances.Add(new ObjectEntity(world, curInstance.instance, graphics, pnt));
            }
            //List<MapInterface.ObjectWall> tempWalls = new List<MapInterface.ObjectWall>(mapInterface.objectWalls);
            mapInterface.objectWalls.Clear();
            ////Convert to wall entities
            //foreach (MapInterface.ObjectWall curInstance in tempInstance)
            //{
            //    prim.Point pnt = new prim.Point(curInstance.x, curInstance.y);
            //    prim.Size size = new prim.Size(curInstance.w, curInstance.y);
            //    mapInterface.objectWalls.Add(new WallEntity(world, curInstance.instance, graphics, pnt, size));
            //}
        }

        public override void Result(ContactResult point)
        {
            base.Result(point);
        }

        public override void Persist(ContactPoint point)
        {
            base.Persist(point);
        }

        public override void Remove(ContactPoint point)
        {
            base.Remove(point);
        }

        public override void Add(ContactPoint point)
        {
            base.Add(point);
        }

        public void CheckContacts()
        {
        }

        public void Draw()
        {
            //TODO: Quad Tree Data Structure
            foreach (ObjectEntity curInstance in mapInterface.objectInstances)
            {
                curInstance.Draw();
            }
            controlEntity.Draw();
        }

        public void Update()
        {
            float dt = 1f / (float)graphics.fps;
            world.SetContinuousPhysics(true);
            world.Step(dt, 8, 3);
            world.Validate();
            controlEntity.SetVector();
            //TODO: Quad Tree Data Structure
            foreach (ObjectEntity curInstance in mapInterface.objectInstances)
            {
                curInstance.Update();
            }
            controlEntity.Update();
        }

        public void CleanUp()
        {
            //DetectCollision();
            //TODO: Quad Tree Data Structure
            foreach (ObjectEntity curInstance in mapInterface.objectInstances)
            {
                curInstance.CleanUp();
            }
            controlEntity.CleanUp();
        }
    }

    class ControlEntity
    {
        private gfx.Graphics graphics;
        public bool active;
        public float vx;
        public float vy;
        public ObjectEntity controlObject;

        public ControlEntity(World world, MapInterface.ObjectTemplate template, gfx.Graphics graphics)
        {
            this.active = true;
            this.graphics = graphics;
            controlObject = new ObjectEntity(world, template, graphics, new prim.Point(0f, 0f), true);
            vx = 0.5f;
            vy = 0.5f;
        }

        public ControlEntity()
        {
            this.active = false;
        }

        public void SetVector()
        {
            if(this.active)
            {
                float nvx = this.vx;
                float nvy = this.vy;
                if (graphics.actionKeyBuffer.Contains((uint)Key.W))
                    controlObject.dy = nvy * -1;
                if (graphics.actionKeyBuffer.Contains((uint)Key.S))
                    controlObject.dy = nvy;
                if (graphics.actionKeyBuffer.Contains((uint)Key.A))
                    controlObject.dx = nvx * -1;
                if (graphics.actionKeyBuffer.Contains((uint)Key.D))
                    controlObject.dx = nvx;
            }
        }

        public void Update()
        {
            if (this.active)
            {
                controlObject.Update();
            }
        }

        public void Draw()
        {
            if(this.active)
            {
                controlObject.Draw();
            }
        }


        public void CleanUp()
        {
            controlObject.dx = 0f;
            controlObject.dy = 0f;
        }

    }

    class CollisionEntity
    {
        public prim.Point pntA;
        public prim.Point pntB;

        public CollisionEntity(prim.Point pntA, prim.Point pntB)
        {
            this.pntA = pntA;
            this.pntB = pntB;
        }
    }

    class WallEntity : MapInterface.ObjectWall
    {
        public ImageTexture texture;
        public prim.Point point;
        public prim.Size size;
        public List<CollisionEntity> colEntities;
        public float dx;
        public float dy;
        public Body body;
        private List<MapInterface.Line> colLines;
        private WallStrategy wallStrategy;

        public WallEntity(World world, MapInterface.ObjectTemplate temp, Graphics graphics, prim.Point point, prim.Size size, bool isDynamic = false)
            : base()
        {
            this.point = point;
            this.x = point.x;
            this.y = point.y;
            this.dx = 0f;
            this.dy = 0f;
            this.colEntities = new List<CollisionEntity>();
            this.instance = temp;
            this.size = size;
            wallStrategy = new WallStrategy();
            wallStrategy.InitTexture(ref world, ref body, this.point, ref this.size, ref texture, ref instance, graphics, isDynamic);
        }

        public void UpdatePoint(prim.Point pnt)
        {
            body.SetXForm(new Vec2(pnt.x, pnt.y), 0f);
        }

        public override void Update()
        {
            point.x = body.GetPosition().X;
            point.y = body.GetPosition().Y;
            this.x = point.x;
            this.y = point.y;
            float[] vertices = {
                 point.x + size.w,  point.y + size.h, 0.0f, 1.0f, 1.0f,  // top right
                 point.x + size.w, point.y, 0.0f, 1.0f, 0.0f,  // bottom right
                point.x, point.y, 0.0f, 0.0f, 0.0f,  // bottom left
                point.x,  point.y + size.h, 0.0f, 0.0f, 1.0f   // top left
            };
            texture.Update(vertices);
        }

        public void CleanUp()
        {
            this.dx = 0f;
            this.dy = 0f;
        }

        public override void Draw()
        {
            texture.Draw();
        }
    }

    class ObjectEntity : MapInterface.ObjectInstance
    {
        public ImageTexture texture;
        public prim.Point point;
        public prim.Size size;
        public List<CollisionEntity> colEntities;
        public float dx;
        public float dy;
        public Body body;
        private ObjectStrategy objectStrategy;

        public ObjectEntity(World world, MapInterface.ObjectTemplate temp, Graphics graphics, prim.Point point, bool isDynamic = false) : base()
        {
            this.point = point;
            this.x = point.x;
            this.y = point.y;
            this.dx = 0f;
            this.dy = 0f;
            this.colEntities = new List<CollisionEntity>();
            this.instance = temp;
            objectStrategy = new ObjectStrategy();
            objectStrategy.InitTexture(ref world, ref body, this.point, ref this.size, ref texture, ref this.instance, graphics, isDynamic);
            //this.InitTexture(world, temp, graphics, isDynamic);
        }

        public void UpdatePoint(prim.Point pnt)
        {
            body.SetXForm(new Vec2(pnt.x, pnt.y), 0f);
        }

        public override void Update()
        {
            Vec2 vel = body.GetLinearVelocity();
            vel.X = dx;
            vel.Y = dy;
            body.SetLinearVelocity(vel);
            point.x = body.GetPosition().X;
            point.y = body.GetPosition().Y;
            this.x = point.x;
            this.y = point.y;
            float[] vertices = {
                 point.x + size.w,  point.y + size.h, 0.0f, 1.0f, 1.0f,  // top right
                 point.x + size.w, point.y, 0.0f, 1.0f, 0.0f,  // bottom right
                point.x, point.y, 0.0f, 0.0f, 0.0f,  // bottom left
                point.x,  point.y + size.h, 0.0f, 0.0f, 1.0f   // top left
            };
            texture.Update(vertices);
        }



        public void CleanUp()
        {
            this.dx = 0f;
            this.dy = 0f;
        }

        public override void Draw()
        {
            texture.Draw();
        }
    }

    class ObjectStrategy
    {
        public ObjectStrategy()
        {

        }

        protected List<prim.Point> GetDistinctPoints(List<prim.Point> pnts)
        {
            List<prim.Point> newPnts = new List<prim.Point>();
            foreach (prim.Point curPnt in pnts)
            {
                bool dup = false;
                foreach (prim.Point cmpPnt in newPnts)
                {
                    if (cmpPnt == curPnt)
                    {
                        dup = true;
                        break;
                    }
                }
                if (!dup)
                    newPnts.Add(curPnt);
            }
            return newPnts;
        }

        protected void AddCollisionPoints(ref World world, ref MapInterface.ObjectTemplate temp, BodyDef bodyDef, PolygonDef polyDef, List<prim.Point> points, ref Body body)
        {
            int vCnt = -1;
            points = GetDistinctPoints(points);
            foreach (prim.Point curPoint in points)
            {
                vCnt += 1;
                polyDef.Vertices[vCnt].Set(curPoint.x, curPoint.y);
            }

            bodyDef.FixedRotation = true;
            body = world.CreateBody(bodyDef);
            body.CreateShape(polyDef);
            //points = GetDistinctPoints(points);
            //int vCnt = -1;
            //foreach (prim.Point curPoint in points)
            //{
            //    vCnt += 1;
            //    polyDef.Vertices[vCnt].Set(curPoint.x, curPoint.y);
            //}
            //bodyDef.FixedRotation = true;
            //body = world.CreateBody(bodyDef);
            //body.CreateShape(polyDef);
            //if (!temp.isSoft)
            //  body.SetMassFromShapes();
        }

        protected void SetPoints(Graphics graphics, Bitmap bitmap, ref ImageTexture texture, prim.Point point, ref prim.Size size)
        {
            if (size == null)
                size = new prim.Size(bitmap.Width / graphics.size.w, bitmap.Height / graphics.size.h);
            float[] vertices = {
                 point.x + size.w,  point.y + size.h, 0.0f, 1.0f, 1.0f,  // top right
                 point.x + size.w, point.y, 0.0f, 1.0f, 0.0f,  // bottom right
                point.x, point.y, 0.0f, 0.0f, 0.0f,  // bottom left
                point.x,  point.y + size.h, 0.0f, 0.0f, 1.0f   // top left
            };

            uint[] indices = {  // note that we start from 0!
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            };

            texture = new ImageTexture(graphics, bitmap, vertices, indices);
        }

        public virtual void InitTexture(ref World world, ref Body body, prim.Point point, ref prim.Size size, ref ImageTexture texture, ref MapInterface.ObjectTemplate temp, Graphics graphics, bool isDynamic = false)
        {
            Bitmap bitmap = temp.images["default"][0].image;
            SetPoints(graphics, bitmap, ref texture, point, ref size);

            BodyDef bodyDef = new BodyDef();
            PolygonDef polyBox = new PolygonDef();
            PolygonDef polyDef = new PolygonDef();
            bodyDef.Position.Set(point.x, point.y);
            polyDef.Density = 1f;
            polyBox.Density = 1f;
            bodyDef.Angle = 0f;
            polyDef.VertexCount = temp.images["default"][0].collisionVectors.Count;
            polyBox.SetAsBox(size.w / 2, size.h / 2);
            List<prim.Point> points = new List<prim.Point>();
            foreach (MapInterface.Line line in temp.images["default"][0].collisionVectors)
            {
                float nx1 = ((float)line.x1 / bitmap.Width) * (float)size.w;
                float nx2 = ((float)line.x2 / bitmap.Width) * (float)size.w;
                float ny1 = ((float)line.y1 / bitmap.Height) * (float)size.h;
                float ny2 = ((float)line.y2 / bitmap.Height) * (float)size.h;
                points.Add(new prim.Point(nx1, ny1));
                points.Add(new prim.Point(nx2, ny2));
            }
            //TODO: Sort vertices in CCW order
            //Example. Top triangle point -> bottom right -> bottem left
            //points = GetDistinctPoints(points);
            //foreach (prim.Point curPoint in points)
            //{
            //    vCnt += 1;
            //    polyDef.Vertices[vCnt].Set(curPoint.x, curPoint.y);
            //}

            //bodyDef.FixedRotation = true;
            //body = world.CreateBody(bodyDef);
            //body.CreateShape(polyDef);

            AddCollisionPoints(ref world, ref temp, bodyDef, polyDef, points, ref body);

            if (isDynamic)
                body.SetMassFromShapes();
        }
    }

    class WallStrategy : ObjectStrategy
    {
        public WallStrategy() : base()
        {

        }

        public override void InitTexture(ref World world, ref Body body, prim.Point point, ref prim.Size size, ref ImageTexture texture, ref MapInterface.ObjectTemplate temp, Graphics graphics, bool isDynamic = false)
        {
            Bitmap bitmap = temp.images["default"][0].image;
            SetPoints(graphics, bitmap, ref texture, point, ref size);
            BodyDef bodyDef = new BodyDef();
            PolygonDef polyBox = new PolygonDef();
            PolygonDef polyDef = new PolygonDef();
            bodyDef.Position.Set(point.x, point.y);
            polyDef.Density = 1f;
            polyBox.Density = 1f;
            bodyDef.Angle = 0f;
            polyDef.VertexCount = temp.images["default"][0].collisionVectors.Count;
            polyBox.SetAsBox(size.w / 2, size.h / 2);
            List<prim.Point> points = new List<prim.Point>();
            foreach (MapInterface.Line line in temp.images["default"][0].collisionVectors)
            {
                float nx1 = ((float)line.x1 / bitmap.Width) * (float)size.w;
                float nx2 = ((float)line.x2 / bitmap.Width) * (float)size.w;
                float ny1 = ((float)line.y1 / bitmap.Height) * (float)size.h;
                float ny2 = ((float)line.y2 / bitmap.Height) * (float)size.h;
                points.Add(new prim.Point(nx1, ny1));
                points.Add(new prim.Point(nx2, ny2));
            }

            AddCollisionPoints(ref world, ref temp, bodyDef, polyDef, points, ref body);

            if (isDynamic)
                body.SetMassFromShapes();
        }
    }

}
