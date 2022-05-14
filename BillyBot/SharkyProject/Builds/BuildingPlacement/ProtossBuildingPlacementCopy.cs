using SC2APIProtocol;
using Sharky.Builds;
using Sharky.Builds.BuildingPlacement;
using Sharky.Pathing;
using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace BillyBot.Sharky.Builds.BuildingPlacement
{
    public class ProtossBuildingPlacementCopy : IBuildingPlacement
    {
        ActiveUnitData ActiveUnitData;
        SharkyUnitData SharkyUnitData;
        BaseData BaseData;
        DebugService DebugService;
        MapDataService MapDataService;
        BuildingService BuildingService;
        IBuildingPlacement WallOffPlacement;
        ProtossPylonGridPlacement ProtossPylonGridPlacement;
        ProtossProductionGridPlacement ProtossProductionGridPlacement;
        IBuildingPlacement ProtectNexusPylonPlacement;
        TargetingData TargetingData;
        IBuildingPlacement ProtectNexusCannonPlacement;
        BuildOptions BuildOptions;
        IBuildingPlacement ProtossDefensiveGridPlacement;
        IBuildingPlacement ProtossProxyGridPlacement;

        Point2D referencePoint = new Point2D { X= 0, Y= 0 };
        float maxDistance = 50;
        float minimumMineralProximinity = 2;
        WallOffType wallOffType = WallOffType.None;
        private Point2D? BuildPoint;
        bool requireVision = false;

        public ProtossBuildingPlacementCopy(ActiveUnitData activeUnitData, SharkyUnitData sharkyUnitData, BaseData baseData, DebugService debugService, MapDataService mapDataService, BuildingService buildingService, IBuildingPlacement wallOffPlacement, ProtossPylonGridPlacement protossPylonGridPlacement, ProtossProductionGridPlacement protossProductionGridPlacement, IBuildingPlacement protectNexusPylonPlacement, TargetingData targetingData, IBuildingPlacement protectNexusCannonPlacement, BuildOptions buildOptions, IBuildingPlacement protossDefensiveGridPlacement, IBuildingPlacement protossProxyGridPlacement)
        {
            ActiveUnitData = activeUnitData;
            SharkyUnitData = sharkyUnitData;
            BaseData = baseData;
            DebugService = debugService;
            MapDataService = mapDataService;
            BuildingService = buildingService;
            WallOffPlacement = wallOffPlacement;
            ProtossPylonGridPlacement = protossPylonGridPlacement;
            ProtossProductionGridPlacement = protossProductionGridPlacement;
            ProtectNexusPylonPlacement = protectNexusPylonPlacement;
            TargetingData = targetingData;
            ProtectNexusCannonPlacement = protectNexusCannonPlacement;
            BuildOptions = buildOptions;
            ProtossDefensiveGridPlacement = protossDefensiveGridPlacement;
            ProtossProxyGridPlacement = protossProxyGridPlacement;
        }

        public ProtossBuildingPlacementCopy(DefaultSharkyBot defaultSharkyBot)
        {
            ActiveUnitData = defaultSharkyBot.ActiveUnitData;
            SharkyUnitData = defaultSharkyBot.SharkyUnitData;
            BaseData = defaultSharkyBot.BaseData;
            DebugService = defaultSharkyBot.DebugService;
            MapDataService = defaultSharkyBot.MapDataService;
            BuildingService = defaultSharkyBot.BuildingService;
            WallOffPlacement = defaultSharkyBot.WallOffPlacement;
            ProtossPylonGridPlacement = defaultSharkyBot.ProtossPylonGridPlacement;
            ProtossProductionGridPlacement = defaultSharkyBot.ProtossProductionGridPlacement;
            ProtectNexusPylonPlacement = defaultSharkyBot.ProtectNexusPylonPlacement;
            TargetingData = defaultSharkyBot.TargetingData;
            ProtectNexusCannonPlacement = defaultSharkyBot.ProtectNexusCannonPlacement;
            BuildOptions = defaultSharkyBot.BuildOptions;
            ProtossDefensiveGridPlacement = defaultSharkyBot.ProtossDefensiveGridPlacement;
            ProtossProxyGridPlacement = defaultSharkyBot.ProtossProxyGridPlacement;

        }

        //Finds placement of Non-Nexus
        public Point2D FindPlacement(Point2D referencePoint, UnitTypes unitType, int size, bool ignoreResourceProximity = false, float maxDistance = 50, bool requireSameHeight = false, WallOffType wallOffType = WallOffType.None, bool requireVision = false, bool allowBlockBase = false)
        {
            this.referencePoint = referencePoint;
            this.maxDistance = maxDistance;
            this.wallOffType = wallOffType;
            this.requireVision = requireVision;

            if (ignoreResourceProximity) { this.minimumMineralProximinity = 0; };

            if (this.wallOffType != WallOffType.None) {
                BuildPoint = handleWallOff(unitType, size, ignoreResourceProximity, requireSameHeight, allowBlockBase);
                if (BuildPoint != null)
                return BuildPoint;
            }

            if (unitType == UnitTypes.PROTOSS_PYLON)
            {  
                BuildPoint = FindPylonPlacement(requireSameHeight, allowBlockBase);
                if(BuildPoint!=null)
                return BuildPoint;
            }
            else
            {
                BuildPoint = FindProductionPlacement(size, allowBlockBase);
                if (BuildPoint != null)
                return BuildPoint;
            }
            return BuildPoint;
        }

        private Point2D handleWallOff(UnitTypes unitType, int size, bool ignoreResourceProximity, bool requireSameHeight, bool allowBlockBase)
        {
            Point2D point;
            if (wallOffType == WallOffType.Full)
            {
                if (!BuildingService.FullyWalled())
                {
                    point = WallOffPlacement.FindPlacement(referencePoint, unitType, size, ignoreResourceProximity, maxDistance, requireSameHeight, wallOffType, requireVision, allowBlockBase);
                    if (point != null)
                    {
                        return point;
                    }
                }
            }
            else if (wallOffType == WallOffType.Partial)
            {
                if (!BuildingService.PartiallyWalled())
                {
                    point = WallOffPlacement.FindPlacement(referencePoint, unitType, size, ignoreResourceProximity, maxDistance, requireSameHeight, wallOffType, requireVision, allowBlockBase);
                    if (point != null)
                    {
                        return point;
                    }
                }
            }
            return FindPlacement(referencePoint, unitType, size, ignoreResourceProximity, maxDistance, requireSameHeight, WallOffType.None, requireVision, allowBlockBase);
        }

        public Point2D FindPylonPlacement(bool requireSameHeight = false, bool allowBlockBase = false)
        {
            if (!allowBlockBase)
            {
                return FindPylonPlacementNoBlock(requireSameHeight);
            }
            else
            {
                return FindPylonPlacementAllowBlock(requireSameHeight);
            }
        }


        private Point2D FindPylonPlacementNoBlock(bool requireSameHeight = false)
        {
            var spot = ProtossPylonGridPlacement.FindPlacement(referencePoint, maxDistance, minimumMineralProximinity);

            if (spot != null) {  }
            else
            {
                spot = FindPylonPlacementProtectNexus();
                if (spot != null) return spot;
            }
           

            var x = referencePoint.X;
            var y = referencePoint.Y;
            var radius = 1f;

            // start at 12 o'clock then rotate around 12 times, increase radius by 1 until it's more than maxDistance
            while (radius < maxDistance / 2.0)
            {
                var fullCircle = Math.PI * 2;
                var sliceSize = fullCircle / (8.0 + radius);
                var angle = 0.0;
                while (angle + (sliceSize / 2) < fullCircle)
                {
                    var point = new Point2D { X = x + (float)(radius * Math.Cos(angle)), Y = y + (float)(radius * Math.Sin(angle)) };

                    if (BuildingService.AreaBuildable(point.X, point.Y, 1.25f) &&
                        (minimumMineralProximinity == 0 || !BuildingService.BlocksResourceCenter(point.X, point.Y, 1.25f)) &&
                        !BuildingService.Blocked(point.X, point.Y, 1.25f, .1f) && !BuildingService.HasAnyCreep(point.X, point.Y, 1.5f) &&
                        (!requireSameHeight || MapDataService.MapHeight(point) == MapDataService.MapHeight(referencePoint)) &&
                        (minimumMineralProximinity == 0 || !BuildingService.BlocksPath(point.X, point.Y, 1.25f)))
                    {
                        var mineralFields = ActiveUnitData.NeutralUnits.Where(u => SharkyUnitData.MineralFieldTypes.Contains((UnitTypes)u.Value.Unit.UnitType) || SharkyUnitData.GasGeyserTypes.Contains((UnitTypes)u.Value.Unit.UnitType));
                        var squared = (1 + minimumMineralProximinity + .5) * (1 + minimumMineralProximinity + .5);
                        var nexusDistanceSquared = 16f;
                        if (minimumMineralProximinity == 0) { nexusDistanceSquared = 0; }
                        var vector = new Vector2(point.X, point.Y);

                        if (!BaseData.BaseLocations.Any(b => Vector2.DistanceSquared(new Vector2(b.Location.X, b.Location.Y), vector) < 25))
                        {
                            var nexusClashes = ActiveUnitData.SelfUnits.Where(u => (u.Value.Unit.UnitType == (uint)UnitTypes.PROTOSS_NEXUS || u.Value.Unit.UnitType == (uint)UnitTypes.PROTOSS_PYLON) && Vector2.DistanceSquared(u.Value.Position, vector) < squared + nexusDistanceSquared);
                            if (nexusClashes.Count() == 0)
                            {
                                var clashes = mineralFields.Where(u => Vector2.DistanceSquared(u.Value.Position, new Vector2(point.X, point.Y)) < squared);
                                if (clashes.Count() == 0)
                                {
                                    if (Vector2.DistanceSquared(new Vector2(referencePoint.X, referencePoint.Y), new Vector2(point.X, point.Y)) <= maxDistance * maxDistance)
                                    {
                                        DebugService.DrawSphere(new Point { X = point.X, Y = point.Y, Z = 12 });
                                        return point;
                                    }
                                }
                            }
                        }
                    }
                    angle += sliceSize;
                }
                radius += 1;
            }

            return null;

        }

        private Point2D FindPylonPlacementProtectNexus()
        {
            var selfBase = BaseData.BaseLocations.FirstOrDefault(b => (b.Location.X == referencePoint.X && b.Location.Y == referencePoint.Y) && !(b.Location.X == TargetingData.SelfMainBasePoint.X && b.Location.Y == TargetingData.SelfMainBasePoint.Y) && !(b.Location.X == TargetingData.NaturalBasePoint.X && b.Location.Y == TargetingData.NaturalBasePoint.Y));
            if (selfBase != null)
            {
                var pylonLocation = ProtectNexusPylonPlacement.FindPlacement(referencePoint, UnitTypes.PROTOSS_PYLON, 1);
                if(pylonLocation!=null)
                    return pylonLocation;
            }

            return null;
        }

        private Point2D FindPylonPlacementAllowBlock(bool requireSameHeight = false)
        {
            var pylonLocation = FindPylonPlacementProtectNexus();
            if (pylonLocation != null) return pylonLocation;

            var x = referencePoint.X;
            var y = referencePoint.Y;
            var radius = 1f;

            // start at 12 o'clock then rotate around 12 times, increase radius by 1 until it's more than maxDistance
            while (radius < maxDistance / 2.0)
            {
                var fullCircle = Math.PI * 2;
                var sliceSize = fullCircle / (8.0 + radius);
                var angle = 0.0;
                while (angle + (sliceSize / 2) < fullCircle)
                {
                    var point = new Point2D { X = x + (float)(radius * Math.Cos(angle)), Y = y + (float)(radius * Math.Sin(angle)) };

                    if (BuildingService.AreaBuildable(point.X, point.Y, 1.25f) &&
                        (minimumMineralProximinity == 0 || !BuildingService.BlocksResourceCenter(point.X, point.Y, 1.25f)) &&
                        !BuildingService.Blocked(point.X, point.Y, 1.25f, .1f) && !BuildingService.HasAnyCreep(point.X, point.Y, 1.5f) &&
                        (!requireSameHeight || MapDataService.MapHeight(point) == MapDataService.MapHeight(referencePoint)) &&
                        (minimumMineralProximinity == 0 || !BuildingService.BlocksPath(point.X, point.Y, 1.25f)))
                    {
                        var mineralFields = ActiveUnitData.NeutralUnits.Where(u => SharkyUnitData.MineralFieldTypes.Contains((UnitTypes)u.Value.Unit.UnitType) || SharkyUnitData.GasGeyserTypes.Contains((UnitTypes)u.Value.Unit.UnitType));
                        var squared = (1 + minimumMineralProximinity + .5) * (1 + minimumMineralProximinity + .5);
                        var nexusDistanceSquared = 16f;
                        if (minimumMineralProximinity == 0) { nexusDistanceSquared = 0; }
                        var vector = new Vector2(point.X, point.Y);

                        var nexusClashes = ActiveUnitData.SelfUnits.Where(u => (u.Value.Unit.UnitType == (uint)UnitTypes.PROTOSS_NEXUS || u.Value.Unit.UnitType == (uint)UnitTypes.PROTOSS_PYLON) && Vector2.DistanceSquared(u.Value.Position, vector) < squared + nexusDistanceSquared);
                        if (nexusClashes.Count() == 0)
                        {
                            var clashes = mineralFields.Where(u => Vector2.DistanceSquared(u.Value.Position, new Vector2(point.X, point.Y)) < squared);
                            if (clashes.Count() == 0)
                            {
                                if (Vector2.DistanceSquared(new Vector2(referencePoint.X, referencePoint.Y), new Vector2(point.X, point.Y)) <= maxDistance * maxDistance)
                                {
                                    DebugService.DrawSphere(new Point { X = point.X, Y = point.Y, Z = 12 });
                                    return point;
                                }
                            }
                        }
                        
                    }
                    angle += sliceSize;
                }
                radius += 1;
            }


            return null;
        }

        


        public Point2D FindProductionPlacement(int size, bool allowBlockBase = true)
        {
            BuildPoint = null;
            //
            if (findNonBlockingNonPylonSpotSizeThree(size, allowBlockBase))
                if(BuildPoint!=null) return BuildPoint;
            if (findNonPylonSizeTwo(size, allowBlockBase))
                if (BuildPoint != null) return BuildPoint;
            if (findBlockingNonPylonSizeThree(size, allowBlockBase))
                if (BuildPoint != null) return BuildPoint;
            if(trySimple(size, allowBlockBase))
                if (BuildPoint != null) return BuildPoint;


            return FindProductionPlacementTryHarder(size, allowBlockBase);
        }

        private bool findNonBlockingNonPylonSpotSizeThree(float size, bool allowBlockBase)
        {

            if (!allowBlockBase && size == 3)
            {
                var spot = ProtossProductionGridPlacement.FindPlacement(referencePoint, size, maxDistance, minimumMineralProximinity);
                if (spot != null) { BuildPoint = spot; return true; }
            }
            return false;
        }

        private bool findNonPylonSizeTwo(float size, bool allowBlockBase)
        {
            if (size == 2)
            {
                var selfBase = BaseData.BaseLocations.FirstOrDefault(b => (b.Location.X == referencePoint.X && b.Location.Y == referencePoint.Y) && !(b.Location.X == TargetingData.SelfMainBasePoint.X && b.Location.Y == TargetingData.SelfMainBasePoint.Y) && !(b.Location.X == TargetingData.NaturalBasePoint.X && b.Location.Y == TargetingData.NaturalBasePoint.Y));
                if (selfBase != null)
                {
                    var location = ProtectNexusCannonPlacement.FindPlacement(referencePoint, UnitTypes.PROTOSS_PHOTONCANNON, 1);
                    if (location != null)
                    {
                        BuildPoint = location; return true;
                    }
                }
                var gridPlacement = ProtossDefensiveGridPlacement.FindPlacement(referencePoint, UnitTypes.PROTOSS_PHOTONCANNON, (int)size, minimumMineralProximinity == 0, maxDistance, true, wallOffType, requireVision, allowBlockBase);
                if (gridPlacement != null)
                {
                    BuildPoint = gridPlacement; return true;
                }

            }
            return false;
        }

        private bool findBlockingNonPylonSizeThree(float size, bool allowBlockBase)
        {
            if (size == 3 && allowBlockBase)
            {
                var gridPlacement = ProtossProxyGridPlacement.FindPlacement(referencePoint, UnitTypes.PROTOSS_GATEWAY, (int)size, minimumMineralProximinity == 0, maxDistance, true, wallOffType, requireVision, allowBlockBase);
                if (gridPlacement != null)
                {
                    BuildPoint = gridPlacement;
                    return true;
                }
            }
            return false;
        }

        private bool trySimple(float size, bool allowBlockBase)
        {
            var targetVector = new Vector2(referencePoint.X, referencePoint.Y);
            var powerSources = ActiveUnitData.Commanders.Values.Where(c => c.UnitCalculation.Unit.UnitType == (uint)UnitTypes.PROTOSS_PYLON && c.UnitCalculation.Unit.BuildProgress == 1).OrderBy(c => Vector2.DistanceSquared(c.UnitCalculation.Position, targetVector));
            foreach (var powerSource in powerSources)
            {
                if (ShouldTargetBePlacedAroundPowerSource(powerSource, size, allowBlockBase)) 
                    return true;
            
            }
            return false;
        }

        private bool ShouldTargetBePlacedAroundPowerSource(UnitCommander? powerSource, float size, bool allowBlockBase)
        {

            if (!IsTargetInRangeOfPowerSource(referencePoint, powerSource, maxDistance)) return false;


            var powerSourcePoint = new Point2D { X = powerSource.UnitCalculation.Unit.Pos.X, Y = powerSource.UnitCalculation.Unit.Pos.Y };
            var pSX = powerSourcePoint.X;
            var pSY = powerSourcePoint.Y;

            var radius = size / 2f;
            var powerRadius = 7 - (size / 2f);

            // start at 12 o'clock then rotate around 12 times, increase radius by 1 until it's more than powerRadius
            while (radius <= powerRadius)
            {

                if(rotateByRadius(powerSource, size, allowBlockBase, radius, pSX, pSY))
                    return true;
                radius += 1;
            }



            return false;
        }


        private bool IsTargetInRangeOfPowerSource(Point2D target, UnitCommander? powerSource, float maxDistance)
        {
            var powerSourceDistanceSquared = Vector2.DistanceSquared(new Vector2(target.X, target.Y), powerSource.UnitCalculation.Position);
            var maxDistanceSquared = (maxDistance + 8) * (maxDistance + 8);
            if (powerSourceDistanceSquared > maxDistanceSquared)
            {
                return false;
            }
            return true;
        }


        private bool rotateByRadius(UnitCommander? powerSource, float size, bool allowBlockBase, float radius, float pSX, float pSY)
        {
            var fullCircle = Math.PI * 2;
            var sliceSize = fullCircle / 24.0;
            var angle = 0.0;
            while (angle + (sliceSize / 2) < fullCircle)
            {
                var pointInRotation = this.pointInRotation(radius, angle, size, pSX, pSY);

                var vector = new Vector2(pointInRotation.X, pointInRotation.Y);
                var tooClose = IsToCloseToBaseOrWall(allowBlockBase, vector);


                if (!tooClose && !IsBlocked(pointInRotation, size, minimumMineralProximinity))
                {
                    var mineralFields = ActiveUnitData.NeutralUnits.Where((KeyValuePair<ulong, UnitCalculation> u) => SharkyUnitData.MineralFieldTypes.Contains((UnitTypes)u.Value.Unit.UnitType));
                    var squared = (1 + minimumMineralProximinity + (size / 2f)) * (1 + minimumMineralProximinity + (size / 2f));
                    var clashes = mineralFields.Where(u => Vector2.DistanceSquared(u.Value.Position, new Vector2(pointInRotation.X, pointInRotation.Y)) < squared);

                    if (clashes.Count() == 0)
                    {
                        if (Vector2.DistanceSquared(new Vector2(referencePoint.X, referencePoint.Y), new Vector2(pointInRotation.X, pointInRotation.Y)) <= maxDistance * maxDistance && Vector2.DistanceSquared(vector, powerSource.UnitCalculation.Position) <= 36)
                        {
                            DebugService.DrawSphere(new Point { X = pointInRotation.X, Y = pointInRotation.Y, Z = 12 });
                            BuildPoint = pointInRotation;
                            return true;
                        }
                    }
                }

                angle += sliceSize;
            }
            return false;
        }

        private Point2D pointInRotation(float radius, double angle, float size, float pSX, float pSY)
        {
            var point = new Point2D { X = pSX + (float)(radius * Math.Cos(angle)), Y = pSY + (float)(radius * Math.Sin(angle)) };
            point = new Point2D { X = (float)Math.Round(point.X * 2f) / 2f, Y = (float)(Math.Round(point.Y * 2f) / 2f) };

            if (size == 3)
            {
                if (point.X % 1 != .5)
                {
                    point.X -= .5f;
                }
                if (point.Y % 1 != .5)
                {
                    point.Y -= .5f;
                }
            }
            else if (size == 2)
            {
                if (point.X % 1 != 0)
                {
                    point.X -= .5f;
                }
                if (point.Y % 1 != 0)
                {
                    point.Y -= .5f;
                }
            }
            return point;
        }

        private bool IsToCloseToBaseOrWall(bool allowBlockBase, Vector2 vector)
        {
            if (!BuildOptions.AllowBlockWall && MapDataService.MapData?.WallData != null && MapDataService.MapData.WallData.Any(d => d.FullDepotWall != null && d.FullDepotWall.Any(p => Vector2.DistanceSquared(new Vector2(p.X, p.Y), vector) < 25)))
            {
                return true;
            }

            if (!allowBlockBase && BaseData.BaseLocations.Any(b => Vector2.DistanceSquared(new Vector2(b.Location.X, b.Location.Y), vector) < 25))
            {
                return true;
            }
            return false;
        }

        private bool IsBlocked(Point2D pointInRotation, float size, float minimumMineralProximinity)
        {
            //DebugService.SetCamera(new Point { X = pointInRotation.X, Y = pointInRotation.Y, Z = 12 });
            float padding = 0.5f;
            bool sameHeight = BuildingService.SameHeight(pointInRotation.X, pointInRotation.Y, size + padding / 2.0f);
            bool isProximityBlocked = !(minimumMineralProximinity == 0 ||
                    !BuildingService.BlocksResourceCenter(pointInRotation.X, pointInRotation.Y, size + padding / 2.0f));
            bool buildableArea = BuildingService.AreaBuildable(pointInRotation.X, pointInRotation.Y, (size + padding) / 2.0f); //if inside map bonudary returns false
            bool blocked = BuildingService.Blocked(pointInRotation.X, pointInRotation.Y, (size + padding) / 2.0f); //checks if another building blocks it with 0.5 more radius


            bool hasAnyCreep = BuildingService.HasAnyCreep(pointInRotation.X, pointInRotation.Y, size / 2.0f);
            bool blocksPath = BuildingService.BlocksPath(pointInRotation.X, pointInRotation.Y, size / 2.0f);

            bool isBlocked = false;

            if (sameHeight)
            {
                if (isProximityBlocked || !buildableArea || blocked || hasAnyCreep)
                    isBlocked = true;
            }
            else isBlocked = true;

            return isBlocked;
        }



        Point2D FindProductionPlacementTryHarder(float size, bool allowBlockBase)
        {
            var targetVector = new Vector2(referencePoint.X, referencePoint.Y);
            var powerSources = ActiveUnitData.Commanders.Values.Where(c => c.UnitCalculation.Unit.UnitType == (uint)UnitTypes.PROTOSS_PYLON && c.UnitCalculation.Unit.BuildProgress == 1).OrderBy(c => Vector2.DistanceSquared(c.UnitCalculation.Position, targetVector));

            foreach (var powerSource in powerSources)
            {
                if (Vector2.DistanceSquared(new Vector2(referencePoint.X, referencePoint.Y), powerSource.UnitCalculation.Position) > (maxDistance + 16) * (maxDistance + 16))
                {
                    break;
                }

                var x = powerSource.UnitCalculation.Unit.Pos.X;
                var y = powerSource.UnitCalculation.Unit.Pos.Y;
                var radius = size / 2f;
                var powerRadius = 7 - (size / 2f);

                // start at 12 o'clock then rotate around 12 times, increase radius by 1 until it's more than powerRadius
                while (radius <= powerRadius)
                {
                    var fullCircle = Math.PI * 2;
                    var sliceSize = fullCircle / 48f;
                    var angle = 0.0;
                    while (angle + (sliceSize / 2) < fullCircle)
                    {
                        var point = new Point2D { X = x + (float)(radius * Math.Cos(angle)), Y = y + (float)(radius * Math.Sin(angle)) };
                        point = new Point2D { X = (float)Math.Round(point.X * 2f) / 2f, Y = (float)(Math.Round(point.Y * 2f) / 2f) };

                        if (size == 3)
                        {
                            if (point.X % 1 != .5)
                            {
                                point.X -= .5f;
                            }
                            if (point.Y % 1 != .5)
                            {
                                point.Y -= .5f;
                            }
                        }
                        else if (size == 2)
                        {
                            if (point.X % 1 != 0)
                            {
                                point.X -= .5f;
                            }
                            if (point.Y % 1 != 0)
                            {
                                point.Y -= .5f;
                            }
                        }

                        var vector = new Vector2(point.X, point.Y);
                        var tooClose = false;
                        if (!BuildOptions.AllowBlockWall && MapDataService.MapData?.WallData != null && MapDataService.MapData.WallData.Any(d => d.FullDepotWall != null && d.FullDepotWall.Any(p => Vector2.DistanceSquared(new Vector2(p.X, p.Y), vector) < 16)))
                        {
                            tooClose = true;
                        }

                        if (!allowBlockBase && BuildingService.BlocksResourceCenter(x, y, size/2f))
                        {
                            tooClose = true;
                        }

                        if (!tooClose && (minimumMineralProximinity == 0 || !BuildingService.BlocksResourceCenter(point.X, point.Y, (size - .5f) / 2.0f)) && !BuildingService.BlocksPath(point.X, point.Y, size / 2f) && BuildingService.AreaBuildable(point.X, point.Y, size / 2.0f) && !BuildingService.Blocked(point.X, point.Y, size / 2.0f, 0) && !BuildingService.HasAnyCreep(point.X, point.Y, size / 2.0f) && !BuildingService.BlocksGas(point.X, point.Y, size / 2.0f))
                        {
                            var mineralFields = ActiveUnitData.NeutralUnits.Where(u => SharkyUnitData.MineralFieldTypes.Contains((UnitTypes)u.Value.Unit.UnitType));
                            var squared = (minimumMineralProximinity + (size / 2f)) * (minimumMineralProximinity + (size / 2f));
                            var clashes = mineralFields.Where(u => Vector2.DistanceSquared(u.Value.Position, new Vector2(point.X, point.Y)) < squared);

                            if (clashes.Count() == 0)
                            {
                                if (Vector2.DistanceSquared(new Vector2(referencePoint.X, referencePoint.Y), new Vector2(point.X, point.Y)) <= maxDistance * maxDistance && Vector2.DistanceSquared(vector, powerSource.UnitCalculation.Position) <= (6.5 - size/2f) * (6.5 - size/2f))
                                {
                                    DebugService.DrawSphere(new Point { X = point.X, Y = point.Y, Z = 12 });
                                    return point;
                                }
                            }
                        }

                        angle += sliceSize;
                    }
                    radius += 1;
                }
            }
            return null;
        }
    }
}
