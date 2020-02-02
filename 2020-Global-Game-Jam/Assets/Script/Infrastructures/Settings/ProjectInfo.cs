// <autogenerated/>

namespace Repair.Infrastructures.Settings
{

#region Music

    public enum MusicType
    {
        Mute,
        Twirly_Tops,

    }

#endregion

#region Music

    public enum SoundType
    {
        Mute,
        Cartoon_Boing,
        cat_screech_frightened,
        Clown_Horn_Squeak,
        Kids_Playing,
        meow_001,
        meow_002,
        meow_003,
        meow_004,
        meow_005,
        meow_006,
        meow_007,

    }

#endregion
    public static class ProjectInfo
    {

#region Scenes

        public static class SceneInfos
        {
            public static readonly SceneInfo Credit = new SceneInfo(buildIndex: 2, name: "Credit", path: "Assets/Scene/Credit.unity", bundleName: "");
            public static readonly SceneInfo Home = new SceneInfo(buildIndex: 0, name: "Home", path: "Assets/Scene/Home.unity", bundleName: "");
            public static readonly SceneInfo Main = new SceneInfo(buildIndex: 1, name: "Main", path: "Assets/Scene/Main.unity", bundleName: "");

        }

#endregion

#region Tags

        public static class TagInfos
        {
            public static readonly TagInfo Untagged = new TagInfo(index: 0, name: "Untagged");
            public static readonly TagInfo Respawn = new TagInfo(index: 1, name: "Respawn");
            public static readonly TagInfo Finish = new TagInfo(index: 2, name: "Finish");
            public static readonly TagInfo EditorOnly = new TagInfo(index: 3, name: "EditorOnly");
            public static readonly TagInfo MainCamera = new TagInfo(index: 4, name: "MainCamera");
            public static readonly TagInfo Player = new TagInfo(index: 5, name: "Player");
            public static readonly TagInfo GameController = new TagInfo(index: 6, name: "GameController");
            public static readonly TagInfo FinishPoint = new TagInfo(index: 7, name: "FinishPoint");

        }

#endregion

#region Layers

        public static class LayerInfos
        {
            public static readonly LayerInfo Default = new LayerInfo(index: 0, name: "Default");
            public static readonly LayerInfo TransparentFX = new LayerInfo(index: 1, name: "TransparentFX");
            public static readonly LayerInfo Ignore_Raycast = new LayerInfo(index: 2, name: "Ignore Raycast");
            public static readonly LayerInfo Water = new LayerInfo(index: 4, name: "Water");
            public static readonly LayerInfo UI = new LayerInfo(index: 5, name: "UI");
            public static readonly LayerInfo Platform = new LayerInfo(index: 8, name: "Platform");
            public static readonly LayerInfo Player = new LayerInfo(index: 9, name: "Player");
            public static readonly LayerInfo Mechanic = new LayerInfo(index: 10, name: "Mechanic");

        }

#endregion

#region SortingLayers

        public static class SortingLayerInfos
        {
            public static readonly SortingLayerInfo Default = new SortingLayerInfo(id: 0, name: "Default");
            public static readonly SortingLayerInfo Background = new SortingLayerInfo(id: 2081649183, name: "Background");
            public static readonly SortingLayerInfo BgFx = new SortingLayerInfo(id: 1112290591, name: "BgFx");
            public static readonly SortingLayerInfo Character = new SortingLayerInfo(id: -2003632115, name: "Character");
            public static readonly SortingLayerInfo CharacterFx = new SortingLayerInfo(id: 1333443455, name: "CharacterFx");
            public static readonly SortingLayerInfo UI = new SortingLayerInfo(id: -1154110915, name: "UI");
            public static readonly SortingLayerInfo UIFx = new SortingLayerInfo(id: -1368343745, name: "UIFx");

        }

#endregion
    }
}
