using CF;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Verse;
using static RimWorld.ThingSetMaker_RandomGeneralGoods;

namespace BiotechnologyAutoAnimalPatch
{
    public class BiotechnologyAutoAnimalPatchMod : Mod
    {
        public BiotechnologyAutoAnimalPatchMod(ModContentPack content) : base(content)
        {
            new Harmony("BiotechnologyAutoAnimalPatch.Mod").PatchAll();
        }
    }

    [HarmonyPatch(typeof(DefGenerator), "GenerateImpliedDefs_PreResolve")]
    public static class GenerateImpliedDefs_PreResolve_Patch
    {
        public static bool generated;
        public static void Prefix()
        {
            if (!generated)
            {
                generated = true;
                var allCultures = DefDatabase<ThingDef>.AllDefs.Where(x => (x.thingCategories?.Any(thingCategory => thingCategory.defName.StartsWith("BiotechnologyCategory")) ?? false)
&& x.comps.Any(compProperties => compProperties is CompProperties_Hatcher)).ToList();
                foreach (var animal in DefDatabase<PawnKindDef>.AllDefs.OrderBy(x => x.race.race.baseBodySize))
                {
                    var existingComp = allCultures.FirstOrDefault(x => x.GetCompProperties<CompProperties_Hatcher>().hatcherPawn == animal);
                    if (existingComp is null && DefDatabase<ThingDef>.GetNamedSilentFail("Capsule" + animal.defName) is null)
                    {
                        if (animal.race.race.Animal || animal.race.race.Insect)
                        {
                            if (animal.race.race.petness >= 1f)
                            {
                                PopulateCulturedPet(animal);
                            }
                            else if (animal.race.race.Insect)
                            {
                                PopulateCulturedInsect(animal);
                            }
                            else if (animal.race.race.baseBodySize <= 0.6f)
                            {
                                PopulateCulturedSmallAnimal(animal);
                            }
                            else if (animal.race.race.baseBodySize <= 2.1f)
                            {
                                PopulateCulturedMediumAnimal(animal);
                            }
                            else
                            {
                                PopulateCulturedLargeAnimal(animal);
                            }
                        }
                    }
                }
            }
        }
        private static void PopulateCulturedPet(PawnKindDef animal)
        {
            var capsuleDef = GetBasicCapsuleDef(animal);
            capsuleDef.thingCategories = new List<ThingCategoryDef>
                {
                    ThingCategoryDef.Named("BiotechnologyCategory_Pet")
                };
            var recipeDef = GetRecipeDefForPet(capsuleDef, animal);
            capsuleDef.PostLoad();
            recipeDef.PostLoad();
            DefDatabase<ThingDef>.Add(capsuleDef);
            DefDatabase<RecipeDef>.Add(recipeDef);
        }

        private static RecipeDef GetRecipeDefForPet(ThingDef capsule, PawnKindDef animal)
        {
            var def = GetBasicRecipeDef(capsule, animal);
            def.recipeUsers = new List<ThingDef>
            {
                ThingDef.Named("Biotechnology_PetLab")
            };
            def.skillRequirements = new List<SkillRequirement>
            {
                new SkillRequirement
                {
                    skill = SkillDefOf.Intellectual,
                    minLevel = 6
                }
            };
            return def;
        }

        private static void PopulateCulturedInsect(PawnKindDef animal)
        {
            var capsuleDef = GetBasicCapsuleDef(animal);
            capsuleDef.thingCategories = new List<ThingCategoryDef>
            {
                ThingCategoryDef.Named("BiotechnologyCategory_Insectoid")
            };
            var recipeDef = GetRecipeDefForInsect(capsuleDef, animal);
            capsuleDef.PostLoad();
            recipeDef.PostLoad();
            DefDatabase<ThingDef>.Add(capsuleDef);
            DefDatabase<RecipeDef>.Add(recipeDef);
        }

        private static RecipeDef GetRecipeDefForInsect(ThingDef capsule, PawnKindDef animal)
        {
            var def = GetBasicRecipeDef(capsule, animal);
            def.recipeUsers = new List<ThingDef>
            {
                ThingDef.Named("Biotechnology_InsectoidsLab")
            };
            def.skillRequirements = new List<SkillRequirement>
            {
                new SkillRequirement
                {
                    skill = SkillDefOf.Intellectual,
                    minLevel = 8
                }
            };
            return def;
        }

        private static void PopulateCulturedSmallAnimal(PawnKindDef animal)
        {
            var capsuleDef = GetBasicCapsuleDef(animal);
            capsuleDef.thingCategories = new List<ThingCategoryDef>
            {
                ThingCategoryDef.Named("BiotechnologyCategory_Small")
            };
            var recipeDef = GetRecipeDefForSmallAnimal(capsuleDef, animal);
            capsuleDef.PostLoad();
            recipeDef.PostLoad();
            DefDatabase<ThingDef>.Add(capsuleDef);
            DefDatabase<RecipeDef>.Add(recipeDef);
        }

        private static RecipeDef GetRecipeDefForSmallAnimal(ThingDef capsule, PawnKindDef animal)
        {
            var def = GetBasicRecipeDef(capsule, animal);
            def.recipeUsers = new List<ThingDef>
            {
                ThingDef.Named("Biotechnology_SmallLab")
            };
            def.skillRequirements = new List<SkillRequirement>
            {
                new SkillRequirement
                {
                    skill = SkillDefOf.Intellectual,
                    minLevel = 6
                }
            };
            return def;
        }

        private static void PopulateCulturedMediumAnimal(PawnKindDef animal)
        {
            var capsuleDef = GetBasicCapsuleDef(animal);
            capsuleDef.thingCategories = new List<ThingCategoryDef>
            {
                ThingCategoryDef.Named("BiotechnologyCategory_Medium")
            };
            var recipeDef = GetRecipeDefForMediumAnimal(capsuleDef, animal);
            capsuleDef.PostLoad();
            recipeDef.PostLoad();
            DefDatabase<ThingDef>.Add(capsuleDef);
            DefDatabase<RecipeDef>.Add(recipeDef);
        }

        private static RecipeDef GetRecipeDefForMediumAnimal(ThingDef capsule, PawnKindDef animal)
        {
            var def = GetBasicRecipeDef(capsule, animal);
            def.recipeUsers = new List<ThingDef>
            {
                ThingDef.Named("Biotechnology_MediumLab")
            };
            def.skillRequirements = new List<SkillRequirement>
            {
                new SkillRequirement
                {
                    skill = SkillDefOf.Intellectual,
                    minLevel = 8
                }
            };
            return def;
        }

        private static void PopulateCulturedLargeAnimal(PawnKindDef animal)
        {
            var capsuleDef = GetBasicCapsuleDef(animal);
            capsuleDef.thingCategories = new List<ThingCategoryDef>
            {
                ThingCategoryDef.Named("BiotechnologyCategory_Large")
            };
            var recipeDef = GetRecipeDefForLargeAnimal(capsuleDef, animal);
            capsuleDef.PostLoad();
            recipeDef.PostLoad();
            DefDatabase<ThingDef>.Add(capsuleDef);
            DefDatabase<RecipeDef>.Add(recipeDef);
        }

        private static RecipeDef GetRecipeDefForLargeAnimal(ThingDef capsule, PawnKindDef animal)
        {
            var def = GetBasicRecipeDef(capsule, animal);
            def.recipeUsers = new List<ThingDef>
            {
                ThingDef.Named("Biotechnology_LargeLab")
            };
            def.skillRequirements = new List<SkillRequirement>
            {
                new SkillRequirement
                {
                    skill = SkillDefOf.Intellectual,
                    minLevel = 10
                }
            };
            return def;
        }
        private static ThingDef GetBasicCapsuleDef(PawnKindDef animal)
        {
            return new ThingDef
            {
                defName = "Capsule" + animal.defName,
                label = animal.label + "Biotechnology.CapsuleEndSentence".Translate(),
                description = "Biotechnology.CultureDescription".Translate(animal.label),
                thingClass = typeof(ThingWithComps),
                graphicData = new GraphicData
                {
                    texPath = "Things/Item/finishcapsule",
                    graphicClass = typeof(Graphic_Single)
                },
                tickerType = TickerType.Normal,
                statBases = new List<StatModifier>
                {
                    new StatModifier
                    {
                        stat = StatDefOf.MaxHitPoints,
                        value = 50
                    },
                    new StatModifier
                    {
                        stat = StatDefOf.Flammability,
                        value = 0.2f
                    },
                    new StatModifier
                    {
                        stat = StatDefOf.DeteriorationRate,
                        value = 10
                    },
                    new StatModifier
                    {
                        stat = StatDefOf.MarketValue,
                        value = animal.race.BaseMarketValue + 80
                    },
                    new StatModifier
                    {
                        stat = StatDefOf.Mass,
                        value = 2
                    },
                    new StatModifier
                    {
                        stat = StatDefOf.Nutrition,
                        value = 0.25f
                    },
                    new StatModifier
                    {
                        stat = StatDefOf.FoodPoisonChanceFixedHuman,
                        value = 0.02f
                    },
                    new StatModifier
                    {
                        stat = StatDefOf.Beauty,
                        value = -4
                    }
                },
                socialPropernessMatters = true,
                comps = new List<CompProperties>
                {
                    new CompProperties_Forbiddable(),
                    new CompProperties_Rottable
                    {
                        daysToRotStart = 15,
                        rotDestroys = true,
                        disableIfHatcher = true,
                    },
                    new CompProperties_TemperatureRuinable
                    {
                        minSafeTemperature = -25,
                        maxSafeTemperature = 40,
                        progressPerDegreePerTick = 0.00005f
                    },
                    new CompProperties_Hatcher
                    {
                        hatcherDaystoHatch = animal.race.race.baseBodySize * 3f,
                        hatcherPawn = animal
                    }
                },
                modExtensions = new List<DefModExtension>
                {
                    new HatcheeForcedPlayerFaction()
                },
                stackLimit = 1,
                ingestible = new IngestibleProperties
                {
                    preferability = FoodPreferability.DesperateOnly,
                    foodType = FoodTypeFlags.AnimalProduct,
                    ingestEffect = EffecterDefOf.EatMeat,
                    ingestSound = SoundDef.Named("RawMeat_Eat"),
                    tasteThought = ThoughtDefOf.AteRawFood
                },
                allowedArchonexusCount = 10,
                healthAffectsPrice = false,
                category = ThingCategory.Item,
                drawerType = DrawerType.MapMeshOnly,
                resourceReadoutPriority = ResourceCountPriority.Middle,
                useHitPoints = true,
                selectable = true,
                altitudeLayer = AltitudeLayer.Item,
                alwaysHaulable = true,
                drawGUIOverlay = true,
                rotatable = false,
                pathCost = 14
            };
        }
        private static RecipeDef GetBasicRecipeDef(ThingDef capsule, PawnKindDef animal)
        {
            return new RecipeDef
            {
                workSpeedStat = StatDefOf.GeneralLaborSpeed,
                effectWorking = EffecterDefOf.Surgery,
                soundWorking = SoundDefOf.Recipe_Surgery,
                unfinishedThingDef = ThingDef.Named("UnfinishedCultureMedium"),
                jobString = "Culturing.",
                workSkill = SkillDefOf.Crafting,
                fixedIngredientFilter = new ThingFilter
                {
                    categories = new List<string>
                    {
                        "MeatRaw"
                    },
                    thingDefs = new List<ThingDef>
                    {
                        ThingDef.Named("Neutroamine"),
                        ThingDef.Named("EmptyCapsule"),
                    }
                },
                defName = "Make_Capsule" + animal.defName,
                label = "Biotechnology.MakeRecipeLabel".Translate(animal.label),
                description = "Biotechnology.MakeRecipeDescription".Translate(animal.label),
                workAmount = 6666 * animal.race.race.baseBodySize,
                ingredients = new List<IngredientCount>
                {
                    new IngredientCount
                    {
                        filter = new ThingFilter
                        {
                            categories = new List<string>
                            {
                                "MeatRaw"
                            }
                        },
                        count = 140 * animal.race.race.baseBodySize
                    },
                    new IngredientCount
                    {
                        filter = new ThingFilter
                        {
                            thingDefs = new List<ThingDef>
                            {
                                ThingDef.Named("Neutroamine")
                            }
                        },
                        count = Mathf.Max(1, (int)animal.race.race.baseBodySize)
                    },
                    new IngredientCount
                    {
                        filter = new ThingFilter
                        {
                            thingDefs = new List<ThingDef>
                            {
                                ThingDef.Named("EmptyCapsule")
                            }
                        },
                        count = Mathf.Max(1, (int)animal.race.race.baseBodySize)
                    }
                },
                products = new List<ThingDefCountClass>
                {
                    new ThingDefCountClass
                    {
                        thingDef = capsule,
                        count = 1
                    }
                }
            };
        }
    }
}
