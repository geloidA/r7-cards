namespace Cardmngr;

public interface IModel<TApiModel>
{
    void Update(TApiModel source);
}
