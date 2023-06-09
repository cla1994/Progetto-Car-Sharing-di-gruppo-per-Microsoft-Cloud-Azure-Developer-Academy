public class Waypoint
{
    public string address;
    public Boolean avoid_ferries;

    public Waypoint(string address)
    {
        this.address = address;
        this.avoid_ferries = true;

    }

    public override string ToString()
    {
        return "{ \"waypoint\": { \"address\": \" " + this.address + " \" }, \"routeModifiers\": { \"avoid_ferries\": true}}";
    }
}


public class Origin : Waypoint
{
    public Origin(string address) : base(address){
    }

    public override string ToString()
    {
        return "{ \"waypoint\": { \"address\": \" " + this.address + " \" }, \"routeModifiers\": { \"avoid_ferries\": true}}";
    }
}

public class Destination : Waypoint
{
    public Destination(string address) : base(address)
    {
    }

    public override string ToString()
    {
        return "{ \"waypoint\": { \"address\": \" " + this.address + " \" }}";
    }
}