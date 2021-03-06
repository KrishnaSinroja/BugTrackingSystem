﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackingSystem.Models.RepositoryClasses
{
    public class SQLBugRepository : IBugRepository
    {
        private readonly AppDbContext context;

        public SQLBugRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Bug AddBug(Bug bug)
        {
            context.Bugs.Add(bug);
            context.SaveChanges();
            return bug;
        }

        public Bug DeleteBug(int id)
        {
            Bug bug = context.Bugs.Find(id);

            if (bug != null)
            {
                context.Bugs.Remove(bug);
                var bugComments = context.BugComments.Where(bugComment => bugComment.BugId == bug.BugId);
                context.BugComments.RemoveRange(bugComments);
                context.SaveChanges();
            }
            return bug;
        }

        public IEnumerable<Bug> GetAllBugs()
        {
            return context.Bugs.Include(bug=>bug.SubCat).Include(bug=>bug.SubCat.Cat);
        }

        public IEnumerable<Bug> GetBugWithComments(int Id)
        {
            return context.Bugs.Where(bug=>bug.BugId==Id).Include(bug=>bug.BugComments).Include(bug=>bug.Owner).Include(bug=>bug.SubCat).Include(bug=>bug.SubCat.Cat);
        }

        public IEnumerable<Bug> GetBug(int Id)
        {
            return context.Bugs.Where(bug=>bug.BugId==Id).Include(bug => bug.SubCat).Include(bug => bug.SubCat.Cat);
        }

        public Bug UpdateBug(Bug bugChanges)
        {
            var bug = context.Bugs.Attach(bugChanges);
            bug.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return bugChanges;
        }

        public IEnumerable<Bug> GetAllBugsOfUser(string id)
        {
            return context.Bugs.Where(bug => bug.ApplicationUserId == id).Include(bug=>bug.SubCat).Include(bug=>bug.SubCat.Cat);
        }

        public IEnumerable<Bug> GetAllBugsWithCategory(int id)
        {
            return context.Bugs.Where(bug=>bug.SubCategoryId==id).Include(bug => bug.SubCat).Include(bug => bug.SubCat.Cat);
        }
    }
}
