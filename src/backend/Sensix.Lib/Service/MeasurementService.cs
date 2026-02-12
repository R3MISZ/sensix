using AutoMapper;
using Sensix.Lib.Dtos;
using Sensix.Lib.Entities;
using Sensix.Lib.Mapping;
using Sensix.Lib.Repository;

namespace Sensix.Lib.Service;

public interface IMeasurementService
{
    Task<MeasurementDto> CreateAsync(CreateMeasurementRequest request);
    Task<IReadOnlyList<MeasurementDto>> GetAllAsync();
    Task<MeasurementDto?> GetByIdAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
}
public class MeasurementService : IMeasurementService
{
    private readonly IUnitOfWork _uow;
    private readonly IMeasurementRepository _measurementRepository;
    private readonly IMapper _mapper;

    public MeasurementService(IUnitOfWork uow, IMeasurementRepository measurementRepository, IMapper mapper)
    {
        _uow = uow;
        _measurementRepository = measurementRepository;
        _mapper = mapper;
    }

    public async Task<MeasurementDto> CreateAsync(CreateMeasurementRequest request)
    {
        var measurement = _mapper.Map<Measurement>(request);
        await _measurementRepository.AddAsync(measurement);
        await _uow.SaveChangesAsync();
        return _mapper.Map<MeasurementDto>(measurement);
    }

    public async Task<IReadOnlyList<MeasurementDto>> GetAllAsync()
    {
        var measurements = await _measurementRepository.GetAllAsync();
        return _mapper.Map<IReadOnlyList<MeasurementDto>>(measurements);
    }

    public async Task<MeasurementDto?> GetByIdAsync(Guid id)
    {
        var measurement = await _measurementRepository.GetByIdNoTrackingAsync(id);
        return measurement is null ? null : _mapper.Map<MeasurementDto>(measurement);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var measurement = await _measurementRepository.GetByIdAsync(id);
        if (measurement is null) return false;

        await _measurementRepository.RemoveAsync(measurement);
        await _uow.SaveChangesAsync();
        return true;
    }
}