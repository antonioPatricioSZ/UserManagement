﻿using UserManagement.Domain.Repositories;

namespace UserManagement.Infrastructure.RepositoryAccess;

public sealed class UnitOfWork : IDisposable, IUnitOfWork {

    private readonly UserManagementContext _context;
    private bool _disposed;

    public UnitOfWork(UserManagementContext context) {
        _context = context;
    }

    public async Task Commit() {
        await _context.SaveChangesAsync();
    }

    public void Dispose() {
        Dispose(true);
    }

    private void Dispose(bool disposing) {

        if (!_disposed && disposing) {
            _context.Dispose();

        }
        _disposed = true;
    }

}
