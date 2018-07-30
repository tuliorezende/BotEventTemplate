﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BotEventManagement.Services.Model.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BotEventManagement.Api.Controllers
{
    /// <summary>
    /// Manage Event Participants of an event
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EventParticipantsController : ControllerBase
    {
        /// <summary>
        /// Get Event Participants of an event
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        /// <summary>
        /// Get a specific Event Participants of an event
        /// </summary>
        /// <param name="participantId"></param>
        /// <returns></returns>
        [HttpGet, Route("{participantId}")]
        public IActionResult Get([FromRoute]string participantId)
        {
            return Ok();
        }

        /// <summary>
        /// Update a specific Event Participant of an event
        /// </summary>
        /// <param name="participantId"></param>
        /// <param name="eventParticipants"></param>
        /// <returns></returns>
        [HttpPut("{participantId}")]
        public IActionResult Put([FromRoute] string participantId, [FromBody] EventParticipants eventParticipants)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok();
        }

        /// <summary>
        /// Create an Event Participant of an event
        /// </summary>
        /// <param name="eventParticipants"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] EventParticipants eventParticipants)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok();
        }
        /// <summary>
        /// Remove an Event Participant of an event
        /// </summary>
        /// <param name="participantId"></param>
        /// <returns></returns>
        [HttpDelete("{participantId}")]
        public IActionResult Delete([FromRoute] string participantId)
        {
            return Ok();
        }

        /// <summary>
        /// Post a sheet to create some Event Participants
        /// </summary>
        /// <param name="participantsSheet"></param>
        /// <returns></returns>
        [HttpPost, Route("file")]
        public IActionResult PostFile(byte[] participantsSheet)
        {
            return Ok();
        }
    }
}