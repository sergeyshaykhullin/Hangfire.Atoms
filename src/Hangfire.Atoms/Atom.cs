﻿using System;
using System.ComponentModel;
using Hangfire.Atoms.Builder;
using Hangfire.States;

namespace Hangfire.Atoms
{
    public static partial class Atom
    {
        public static string Enqueue(this IBackgroundJobClient client, string name, Action<IAtomBuilder> buildAtom)
        {
            var builder = new AtomBuilder(name, JobStorage.Current, client, buildAtom);
            return builder.Build();
        }

        public static string Schedule(this IBackgroundJobClient client, string name, TimeSpan enqueueIn, Action<IAtomBuilder> buildAtom)
        {
            var builder = new AtomBuilder(name, JobStorage.Current, client, buildAtom, new ScheduledState(enqueueIn));
            return builder.Build();
        }

        public static string Schedule(this IBackgroundJobClient client, string name, DateTime enqueueAt, Action<IAtomBuilder> buildAtom)
        {
            var builder = new AtomBuilder(name, JobStorage.Current, client, buildAtom, new ScheduledState(enqueueAt));
            return builder.Build();
        }

        public static string ContinueJobWith(this IBackgroundJobClient client, string parentId, string name, Action<IAtomBuilder> buildAtom)
        {
            var builder = new AtomBuilder(name, JobStorage.Current, client, buildAtom, new AwaitingState(parentId));
            return builder.Build();
        }

        public static string OnTriggerSet(this IBackgroundJobClient client, string triggerName, string name, Action<IAtomBuilder> buildAtom)
        {
            var triggerId = client.OnTriggerSet(triggerName);
            var builder = new AtomBuilder(name, JobStorage.Current, client, buildAtom, new AwaitingState(triggerId));
            return builder.Build();
        }

        internal static string GenerateAtomKey(string jobId) => "atom:" + jobId;

        internal static string GenerateSubAtomKeys(string jobId) => "atom:subs:" + jobId;

        internal static string GenerateSubAtomRemainingKeys(string jobId) => "atom:subs:left:" + jobId;

        [DisplayName("{0}")]
        public static void Running(string name)
        {
        }

        public static void NoMethod()
        {
        }
    }
}
