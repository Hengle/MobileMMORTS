﻿using Assets.Code.AssetHandling;
using Assets.Code.Net;
using Client.Net;
using CommonCode.EntityShared;
using MapHandler;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Game.Factories
{
    public class PlayerFactory
    {
        public static void BuildAndInstantiate(PlayerFactoryOptions opts)
        {
            var playerObj = GameObject.Find(opts.UserId);
            if (playerObj == null)
            {
                // Player Obj
                playerObj = new GameObject(opts.UserId);
                playerObj.transform.localScale = new Vector3(100, 100);
                playerObj.tag = "Player";
                FactoryMethods.AddCollider(playerObj);
                if (opts.IsMainPlayer)
                {
                    playerObj.AddComponent<PlayerBehaviour>();
                    UnityClient.Player.Speed = opts.Speed;
                    UnityClient.Player.PlayerObject = playerObj;
                    UnityClient.Player.Position = new Position(opts.tileX, opts.tileY);
                }

                // Body
                var bodyObj = new GameObject("body");
                bodyObj.transform.localScale = new Vector3(100, 100);
                var spriteRenderer = bodyObj.AddComponent<SpriteRenderer>();
                var spriteSheet = bodyObj.AddComponent<SpriteSheet>();
                spriteRenderer.sortingOrder = 2;
                var bodySprite = AssetHandler.LoadedAssets[DefaultAssets.SPR_BODIES];
                var spriteRow = bodySprite.SliceRow(opts.BodySpriteIndex).ToArray();
                spriteSheet.SetSheet(spriteRow);
                spriteRenderer.sprite = spriteSheet.WalkSouth[1];
                bodyObj.transform.parent = playerObj.transform;

                // LEGS
                var legsObj = new GameObject("legs");
                legsObj.transform.localScale = new Vector3(100, 100);
                var legsSpriteRenderer = legsObj.AddComponent<SpriteRenderer>();
                var legsSpriteSheet = legsObj.AddComponent<SpriteSheet>();
                legsSpriteRenderer.sortingOrder = 3;
                var legsSprite = AssetHandler.LoadedAssets[DefaultAssets.SPR_LEGS];
                var legsSpriteRow = legsSprite.SliceRow(opts.LegsSpriteIndex).ToArray();
                legsSpriteSheet.SetSheet(legsSpriteRow);
                legsSpriteRenderer.sprite = legsSpriteSheet.WalkSouth[1];
                legsObj.transform.parent = playerObj.transform;

                // Chest
                var chestObj = new GameObject("chest");
                chestObj.transform.localScale = new Vector3(100, 100);
                var chestSpriteRenderer = chestObj.AddComponent<SpriteRenderer>();
                var chestSpriteSheet = chestObj.AddComponent<SpriteSheet>();
                chestSpriteRenderer.sortingOrder = 4;
                var chestSprite = AssetHandler.LoadedAssets[DefaultAssets.SPR_CHESTS];
                var chestSpriteRow = chestSprite.SliceRow(opts.ChestSpriteIndex).ToArray();
                chestSpriteSheet.SetSheet(chestSpriteRow);
                chestSpriteRenderer.sprite = chestSpriteSheet.WalkSouth[1];
                chestObj.transform.parent = playerObj.transform;

                // Head
                var headObj = new GameObject("head");
                headObj.transform.localScale = new Vector3(100, 100);
                var headSpriteRenderer = headObj.AddComponent<SpriteRenderer>();
                var headSpriteSheet = headObj.AddComponent<SpriteSheet>();
                headSpriteRenderer.sortingOrder = 5;
                var headSprite = AssetHandler.LoadedAssets[DefaultAssets.SPR_HEADS];
                var headSpriteRow = headSprite.SliceRow(opts.HeadSpriteIndex).ToArray();
                headSpriteSheet.SetSheet(headSpriteRow);
                headSpriteRenderer.sprite = headSpriteSheet.WalkSouth[1];
                headObj.transform.parent = playerObj.transform;

                // This gotta be in the end
                playerObj.transform.position = new Vector2(opts.tileX * 16, -opts.tileY * 16);

                if (!opts.IsMainPlayer)
                { 
                    var movingEntity = playerObj.AddComponent<MovingEntityBehaviour>();
                    movingEntity.MoveSpeed = opts.Speed;
                    movingEntity.MapPosition = new Position(opts.tileX, opts.tileY);
                    movingEntity.SpriteSheets.Add(spriteSheet);
                    movingEntity.SpriteSheets.Add(headSpriteSheet);
                    movingEntity.SpriteSheets.Add(legsSpriteSheet);
                    movingEntity.SpriteSheets.Add(chestSpriteSheet);
                    var playerWrapper = new PlayerWrapper()
                    {
                        PlayerObject = playerObj
                    };
                    movingEntity.EntityWrapper = playerWrapper;
                    UnityClient.Map.EntityPositions.AddEntity(movingEntity.EntityWrapper, new Position(opts.tileX, opts.tileY));
                }
            }
        }
    }

    public class PlayerFactoryOptions
    {
        public bool IsMainPlayer;
        public string UserId;
        public int BodySpriteIndex;
        public int ChestSpriteIndex;
        public int HeadSpriteIndex;
        public int LegsSpriteIndex;
        public int tileX;
        public int tileY;
        public int Speed;
    }
}
