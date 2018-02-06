public class BFSMethod : MazeMethod {
    public BFSMethod (int w, int h) : base (w, h) { }
    override public Cell[, ] GenerateMaze () {
        return maze;
    }

    // do the samething with a queue.
}