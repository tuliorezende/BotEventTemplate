﻿using System.Threading.Tasks;
using BotEventManagement.Models.API;
using BotEventManagement.Web.Api;
using BotEventManagement.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BotEventManagement.Web.Controllers
{
    [CustomAuthorization]
    public class ActivityController : Controller
    {
        private readonly IEventManagerApi _eventManagerApi;
        public ActivityController(IEventManagerApi eventManagerApi)
        {
            _eventManagerApi = eventManagerApi;
        }
        // GET: Speaker
        /// <summary>
        /// Method to return all speakers of an event
        /// </summary>
        /// <param name="id">event Id</param>
        /// <returns></returns>
        public async Task<ActionResult> Index(string id)
        {
            var speakers = await _eventManagerApi.GetAllActivitiesOfAnEventsAsync(id);

            TempData["EventId"] = id;
            TempData.Keep("EventId");

            return View(speakers);
        }

        // GET: Speaker/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var details = await _eventManagerApi.GetAnActivityOfAnEventAsync(TempData.Peek("EventId").ToString(), id);
            return View(details);
        }

        // GET: Speaker/Create
        public async Task<ActionResult> Create()
        {
            await CreateStageDropDown();
            await CreateSpeakerDropDown();
            return View();
        }

        // POST: Speaker/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ActivityRequest activityRequest)
        {
            try
            {
                await _eventManagerApi.CreateActivityOfAnEventAsync(TempData.Peek("EventId").ToString(), activityRequest);

                return RedirectToAction(nameof(Index), "Activity", new { id = TempData.Peek("EventId").ToString() });
            }
            catch
            {
                await CreateStageDropDown();
                await CreateSpeakerDropDown();
                return View();
            }
        }

        // GET: Speaker/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var details = await _eventManagerApi.GetAnActivityOfAnEventAsync(TempData.Peek("EventId").ToString(), id);

            await CreateStageDropDown();
            await CreateSpeakerDropDown();

            return View(details);
        }

        // POST: Speaker/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, ActivityRequest activityRequest)
        {
            try
            {
                await _eventManagerApi.UpdateActivityOfAnEventAsync(TempData.Peek("EventId").ToString(), id, activityRequest);
                return RedirectToAction(nameof(Index), "Activity", new { id = TempData.Peek("EventId").ToString() });
            }
            catch
            {
                await CreateStageDropDown();
                await CreateSpeakerDropDown();
                return View();
            }
        }

        private async Task CreateSpeakerDropDown()
        {
            ViewBag.SpeakerId = new SelectList(await _eventManagerApi.GetAllSpeakersOfAnEventsAsync(TempData.Peek("EventId").ToString()), "SpeakerId", "Name");
        }
        private async Task CreateStageDropDown()
        {
            ViewBag.StageId = new SelectList(await _eventManagerApi.GetAllStagesOfAnEventsAsync(TempData.Peek("EventId").ToString()), "StageId", "Name");
        }
    }
}