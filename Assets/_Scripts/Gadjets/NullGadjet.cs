using System.Collections;

public class NullGadjet : Gadjet
{
    public override IEnumerator Ability()
    {
        throw new System.NotImplementedException();
    }
}